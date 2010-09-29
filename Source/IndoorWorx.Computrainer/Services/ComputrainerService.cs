using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace IndoorWorx.Computrainer.Services
{
    public enum Constants
    {
        ReadTimeout = 1000,
        WriteTimeout = 2000
    }

    public enum MessageTypes : int
    {
        Speed = 0x01,
        Power = 0x02,
        HeartRate = 0x03,
        Cadence = 0x06,
        RRC = 0x09,
        Sensor = 0x0b
    }

    public enum Buttons
    {
        Reset = 0x01,
        F1 = 0x02,
        F3 = 0x04,
        Plus = 0x08,
        F2 = 0x10,
        Minus = 0x20,
        Sss = 0x40, //spinscan sync is not a button
        None = 0x80
    }

    public enum DeviceOperationModes
    {
        Ergo = 0x01,
        SpinScan = 0x02
    }

    public enum UIOperationModes
    {
        Manual = 0x01,
        Ergo = 0x02
    }

    public enum ControlStatuses
    {
        Terminate = 0,
        Running = 0x01,
        Paused = 0x02
    }    

    class Computrainer
    {
        private readonly DeviceOperationModes DefaultMode = DeviceOperationModes.Ergo;
        private readonly double DefaultLoad = 100.00;
        private readonly double DefaultGradient = 2.00;
        
        private readonly static byte[] ergo_command = new byte[] 
        {
    //                        Ergo            various
    //      crc     -     -   mode  cmd   val   bits
    //      ----  ----  ----  ----  ----  ----  ----
            0x6D, 0x00, 0x00, 0x0A, 0x08, 0x00, 0xE0,
            0x65, 0x00, 0x00, 0x0A, 0x10, 0x00, 0xE0,
            0x00, 0x00, 0x00, 0x0A, 0x18, 0x5D, 0xC1,
            0x33, 0x00, 0x00, 0x0A, 0x24, 0x1E, 0xE0,
            0x6A, 0x00, 0x00, 0x0A, 0x2C, 0x5F, 0xE0,
            0x41, 0x00, 0x00, 0x0A, 0x34, 0x00, 0xE0,
            0x2D, 0x00, 0x00, 0x0A, 0x38, 0x10, 0xC2,
            0x03, 0x00, 0x00, 0x0A, 0x40, 0x32, 0xE0    // set LOAD command
        };

        private readonly static byte[] ss_command = new byte[] {
    //                      Spinscan           various
    //      crc     -     -   mode  cmd   val   bits
    //      ----  ----  ----  ----  ----  ----  ----
            0x61, 0x00, 0x00, 0x16, 0x08, 0x00, 0xE0,   // set GRADIENT command
            0x59, 0x00, 0x00, 0x16, 0x10, 0x00, 0xE0,   // set WINDSPEED (?)
            0x74, 0x00, 0x00, 0x16, 0x18, 0x5D, 0xC1,
            0x27, 0x00, 0x00, 0x16, 0x24, 0x1E, 0xE0,
            0x5E, 0x00, 0x00, 0x16, 0x2C, 0x5F, 0xE0,
            0x35, 0x00, 0x00, 0x16, 0x34, 0x00, 0xE0,
            0x21, 0x00, 0x00, 0x16, 0x38, 0x10, 0xC2,
            0x29, 0x00, 0x00, 0x16, 0x40, 0x00, 0xE0
        };

        // INBOUND TELEMETRY - all volatile since it is updated by the run() thread
        private double devicePower;            // current output power in Watts
        private double deviceHeartRate;        // current heartrate in BPM
        private double deviceCadence;          // current cadence in RPM
        private double deviceSpeed;            // current speef in KPH
        private double deviceRRC;              // calibrated Rolling Resistance
        private bool   deviceCalibrated;       // is it calibrated?
        private double[] spinData = new double[24];           // SS values only in SS_MODE
        private int    deviceButtons;          // Button status
        private bool   deviceHRConnected;      // HR jack is connected
        private bool   deviceCadenceConnected;     // Cadence jack is connected
        private ControlStatuses    deviceStatus;           // Device status running, paused, disconnected

        // OUTBOUND COMMANDS - all private since it is updated by the GUI thread
        private DeviceOperationModes mode;
        private double load;
        private double gradient;

        // i/o message holder
        byte[] buf = new byte[7];

        // 56 bytes comprise of 8 7byte command messages, where
        // the last is the set load / gradient respectively
        byte[] ERGO_Command = new byte[56];
        byte[] SS_Command = new byte[56];

        private Mutex pvars = new Mutex();

        private SerialPort devicePort;

        public Computrainer(string device)
        {
            devicePower = deviceHeartRate = deviceCadence = deviceSpeed = deviceRRC = 0;
            for(int i=0;i<24;i++)
                spinData[i] = 0;
            mode = DefaultMode;
            load = DefaultLoad;
            gradient = DefaultGradient;
            deviceCalibrated = false;
            deviceHRConnected = false;
            deviceCadenceConnected = false;
            Device = device;
            deviceStatus = 0;

            Buffer.BlockCopy(ERGO_Command, 0, ergo_command, 0, 56);
            Buffer.BlockCopy(SS_Command, 0, ss_command, 0, 56);
        }

        private string deviceFilename;
        public string Device
        {
            get { return this.deviceFilename; }
            set
            {
                this.deviceFilename = value;
            }
        }

        public void SetMode(DeviceOperationModes mode, double load, double gradient)
        {
            lock (pvars)
            {
                this.mode = mode;
                this.load = load;
                this.gradient = gradient;
            }
        }

        public void SetLoad(double load)
        {
            lock (pvars)
            {
                this.load = load;
            }
        }

        public void SetGradient(double gradient)
        {
            lock (pvars)
            {
                this.gradient = gradient;
            }
        }

        public bool IsHRConnected
        {
            get
            {
                lock (pvars)
                {
                    return this.deviceHRConnected;
                }
            }
        }

        public bool IsCadenceConnected
        {
            get
            {
                lock (pvars)
                {
                    return this.deviceCadenceConnected;
                }
            }
        }

        public bool IsCalibrated
        {
            get
            {
                lock (pvars)
                {
                    return this.deviceCalibrated;
                }
            }
        }

        public double Load
        {
            get
            {
                lock (pvars)
                {
                    return this.load;
                }
            }
        }

        public double Gradient
        {
            get
            {
                lock (pvars)
                {
                    return this.gradient;
                }
            }
        }

        public void GetSpinScan(ref double[] spinData)
        {
            lock (pvars)
            {
                int i=0;
                foreach (var ss in this.spinData)
                {
                    spinData[i++] = ss;
                }
            }
        }

        public void GetTelemetry(Telemetry telemetry)
        {
            lock (pvars)
            {
                telemetry.Power = this.devicePower;
                telemetry.HeartRate = this.deviceHeartRate;
                telemetry.Cadence = this.deviceCadence;
                telemetry.Speed = this.deviceSpeed;
                telemetry.RRC = this.deviceRRC;
                telemetry.IsCalibrated = this.deviceCalibrated;
                telemetry.Buttons = this.deviceButtons;
                telemetry.Status = this.deviceStatus;
            }
        }

        public void PrepareCommand(DeviceOperationModes mode, double value)
        {
            byte crc, load;
            int gradient;
            switch (mode)
            {
                case DeviceOperationModes.Ergo:
                    load = Convert.ToByte(value);
                    crc = CalcCRC(load);
                    // BYTE 0 - 49 is b0, 53 is b4, 54 is b5, 55 is b6
                    ERGO_Command[49] = Convert.ToByte(crc >> 1); // set byte 0

                    // BYTE 4 - command and highbyte
                    ERGO_Command[53] = 0x40; // set command
                    ERGO_Command[53] |= Convert.ToByte((load & (2048 + 1024 + 512)) >> 9);

                    // BYTE 5 - low 7
                    ERGO_Command[54] = 0;
                    ERGO_Command[54] |= Convert.ToByte((load & (128 + 64 + 32 + 16 + 8 + 4 + 2)) >> 1);

                    // BYTE 6 - sync + z set
                    ERGO_Command[55] = 128 + 64;

                    // low bit of supplement in bit 6 (32)
                    ERGO_Command[55] |= Convert.ToByte((crc & 1) == 0 ? 0 : 32);// ? 32 : 0;
                    // Bit 2 (0x02) is low bit of high byte in load (bit 9 0x256)
                    ERGO_Command[55] |= Convert.ToByte((load & 256) == 0 ? 0 : 2);// ? 2 : 0;
                    // Bit 1 (0x01) is low bit of low byte in load (but 1 0x01)
                    ERGO_Command[55] |= Convert.ToByte(load & 1);
                    break;
                case DeviceOperationModes.SpinScan:
                    break;
                default:
                    break;
            }
        }

        private byte CalcCRC(byte value)
        {
            return Convert.ToByte((0xff & (107 - (value & 0xff) - (value >> 8))));
        }

        // funny, just a few lines of code. oh the pain to get this working :-)
        public void UnpackTelemetry(ref int ss1, ref int ss2, ref int ss3, ref int buttons, ref int type, ref int value8, ref int value12)
        {
            /* ---- looking at spinscan data -- commented out for release
            static int ss[24];
            static int pos=0;
            ----- */

            // inbound data is in the 7 byte array Computrainer::buf[]
            // for code clarity they hjave been put into these holdiing
            // variables. the overhead is minimal and makes the code a
            // lot easier to decipher! :-)

            short s1 = buf[0]; // ss data
            short s2 = buf[1]; // ss data
            short s3 = buf[2]; // ss data
            short bt = buf[3]; // button data
            short b1 = buf[4]; // message and value
            short b2 = buf[5]; // value
            short b3 = buf[6]; // the dregs (sync, z and lsb for all the others)

            // ss vars
            ss1 = s1<<1 | (b3&32)>>5;
            ss2 = s2<<1 | (b3&16)>>4;
            ss3 = s3<<1 | (b3&8)>>3;


            // buttons
            buttons = bt<<1 | (b3&4)>>2;

            // 4-bit message type
            type = (b1&120)>>3;

            // 8 bit value
            value8 = (b2&~128)<<1 | (b3&1); // 8 bit values

            // 12 bit value
            value12 = value8 | (b1&7)<<9 | (b3&2)<<7;

            /* ------- Looking at spinscan data ? -- commented out for release
            if (buttons&64) {
                for (pos=0; pos<24; pos++) fprintf(stderr, "%d, ", ss[pos]);
                pos=0;
                fprintf(stderr, "\n");
            }
            if (ss1 || ss2 || ss3) {
                ss[pos++] = ss1;
                ss[pos++] = ss2;
                ss[pos++] = ss3;
            }
            ------- */
        }

        /* ----------------------------------------------------------------------
         * EXECUTIVE FUNCTIONS
         *
         * start() - start/re-start reading telemetry in a thread
         * stop() - stop reading telemetry and terminates thread
         * pause() - discards inbound telemetry (ignores it)
         *
         *
         * THE MEAT OF THE CODE IS IN RUN() IT IS A WHILE LOOP CONSTANTLY
         * READING TELEMETRY AND ISSUING CONTROL COMMANDS WHILST UPDATING
         * MEMBER VARIABLES AS TELEMETRY CHANGES ARE FOUND.
         *
         * run() - bg thread continuosly reading/writing the device port
         *         it is kicked off by start and then examines status to check
         *         when it is time to pause or stop altogether.
         * ---------------------------------------------------------------------- */
        public int Restart()
        {
            ControlStatuses status;

            lock(pvars)
            {
                status = this.deviceStatus;
            }
            if((status&ControlStatuses.Running) == ControlStatuses.Running && (status&ControlStatuses.Paused) == ControlStatuses.Paused)
            {
                status &= ControlStatuses.Paused;
                lock(pvars)
                {
                    this.deviceStatus = status;
                }
                return 0;
            }
            return 2;
        }

        public int Stop()
        {
            ControlStatuses status;
            lock(pvars)
            {
                status = this.deviceStatus;
            }
            lock(pvars)
            {
                deviceStatus = ControlStatuses.Terminate;
            }
            return 0;
        }

        public int Pause()
        {
            ControlStatuses status;
            lock(pvars)
            {
                status = this.deviceStatus;
            }
            if((status & ControlStatuses.Paused) == ControlStatuses.Paused) 
                return 2;
            else if(!((status & ControlStatuses.Running) == ControlStatuses.Running))
                return 4;
            else
            {
                status |= ControlStatuses.Paused;
                lock(pvars)
                {
                    this.deviceStatus = status;
                }
                return 0;
            }
        }

        public void Run()
        {

            // locally cached settings - only update main class variables
            //                           when they change

            int cmds = 0;            // count loops with no command sent

            // holders for unpacked telemetry
            int ss1, ss2, ss3, buttons, type, value8, value12;
            ss1 = 0; ss2 = 0; ss3 = 0; buttons = 0; type = 0; value8 = 0; value12 = 0;

            // newly read values - compared against cached values
            int changed;
            DeviceOperationModes newmode;
            double newload, newgradient;
            double newspeed, newRRC;
            bool newcalibrated, newhrconnected, newcadconnected;
            bool isDeviceOpen = false;

            // Cached current values
            // when new values are received from the device
            // if they differ from current values we update
            // otherwise do nothing
            DeviceOperationModes curmode;
            ControlStatuses curStatus;
            double curload, curgradient;
            double curPower;                      // current output power in Watts
            double curHeartRate;                  // current heartrate in BPM
            double curCadence;                    // current cadence in RPM
            double curSpeed;                      // current speef in KPH
            double curRRC;                        // calibrated Rolling Resistance
            bool curcalibrated;                   // is it calibrated?
            bool curhrconnected;                  // is HR sensor connected?
            bool curcadconnected;                 // is CAD sensor connected?
            double[] curspinData = new double[24];               // SS values only in SS_MODE
            int curButtons;                       // Button status


            // initialise local cache & main vars
            lock (pvars)
            {
                this.deviceStatus = ControlStatuses.Running;
                curmode = this.mode;
                curload = this.load;
                curgradient = this.gradient;
                curPower = this.devicePower = 0;
                curHeartRate = this.deviceHeartRate = 0;
                curCadence = this.deviceCadence = 0;
                curSpeed = this.deviceSpeed = 0;
                curButtons = this.deviceButtons;
                curRRC = this.deviceRRC = 0;
                curcalibrated = false;
                this.deviceCalibrated = false;
                curhrconnected = false;
                this.deviceHRConnected = false;
                curcadconnected = false;
                this.deviceCadenceConnected = false;
                curButtons = 0;
                this.deviceButtons = 0;
                curStatus = this.deviceStatus;
                for (int i = 0; i < 24; i++)
                    curspinData[i] = this.spinData[i] = 0;
            }


            // open the device
            if (!OpenPort())
            {
                return; // open failed!
            }
            else
            {
                isDeviceOpen = true;
            }

            // send first command to get computrainer ready
            PrepareCommand(curmode, curmode == DeviceOperationModes.Ergo ? curload : curgradient);
            if (SendCommand(curmode))
            {
                while (true)
                {
                    if (isDeviceOpen)
                    {
                        if (ReadMessage() > 0)
                        {
                            changed = 0;
                            UnpackTelemetry(ref ss1, ref ss2, ref ss3, ref buttons, ref type, ref value8, ref value12);
                            switch (type)
                            {
                                case (int)MessageTypes.Speed:
                                    value12 *= 36;      // convert from mps to kph
                                    value12 *= 9;
                                    value12 /= 10;      // it seems that compcs takes off 10% ????
                                    newspeed = value12;
                                    newspeed /= 1000;
                                    if (newspeed != curSpeed)
                                    {
                                        lock (pvars)
                                        {
                                            this.deviceSpeed = curSpeed = newspeed;
                                        }
                                        changed = 1;
                                    }
                                    break;
                                case (int)MessageTypes.Power:
                                    if (value12 != curPower)
                                    {
                                        curPower = value12;
                                        lock (pvars)
                                        {
                                            this.devicePower = curPower;
                                        }
                                        changed = 1;
                                    }
                                    break;
                                case (int)MessageTypes.HeartRate:
                                    if (value8 != curHeartRate)
                                    {
                                        curHeartRate = value8;
                                        lock (pvars)
                                        {
                                            this.deviceHeartRate = curHeartRate;
                                        }
                                        changed = 1;
                                    }
                                    break;
                                case (int)MessageTypes.Cadence:
                                    if (value8 != curCadence)
                                    {
                                        curCadence = value8;
                                        lock (pvars)
                                        {
                                            this.deviceCadence = curCadence;
                                        }

                                        changed = 1;
                                    }
                                    break;
                                case (int)MessageTypes.RRC:
                                    newcalibrated = (value12 & 2048) != 0 ? true : false;
                                    newRRC = value12 & ~2048; // only use 11bits
                                    newRRC /= 256;

                                    if (newRRC != curRRC)
                                    {
                                        lock (pvars)
                                        {
                                            this.deviceRRC = curRRC = newRRC;
                                        }
                                        changed = 1;
                                    }
                                    break;
                                case (int)MessageTypes.Sensor:
                                    newcadconnected = (value12 & 2048) != 0 ? true : false;
                                    newhrconnected = (value12 & 1024) != 0 ? true : false;

                                    if (newhrconnected != curhrconnected || newcadconnected != curcadconnected)
                                    {
                                        lock (pvars)
                                        {
                                            this.deviceHRConnected = curhrconnected = newhrconnected;
                                            this.deviceCadenceConnected = curcadconnected = newcadconnected;
                                        }
                                        changed = 1;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            if (buttons != curButtons)
                            {
                                lock (pvars)
                                {
                                    this.deviceButtons = curButtons = buttons;
                                }
                            }
                            else
                            {
                                Thread.Sleep(100);
                            }
                            lock (pvars)
                            {
                                curStatus = this.deviceStatus;
                                newmode = this.mode;
                                newload = this.load;
                                curgradient = newgradient = this.gradient;
                            }
                            if ((curStatus & ControlStatuses.Running) == 0)
                            {
                                ClosePort();
                                return;
                            }
                            if ((curStatus & ControlStatuses.Paused) != 0 && isDeviceOpen)
                            {
                                ClosePort();
                                isDeviceOpen = false;
                            }
                            else if ((curStatus & ControlStatuses.Paused) == 0 && (curStatus & ControlStatuses.Running) != 0 && isDeviceOpen == false)
                            {
                                if (!OpenPort())
                                {
                                    return;
                                }
                                isDeviceOpen = true;
                                PrepareCommand(curmode, curmode == DeviceOperationModes.Ergo ? curload : curgradient);
                                if (!SendCommand(curmode))
                                {
                                    ClosePort();
                                    return;
                                }
                                if (isDeviceOpen && (cmds % 10) == 0)
                                {
                                    cmds = 1;
                                    curmode = newmode;
                                    curload = newload;
                                    curgradient = newgradient;
                                    PrepareCommand(curmode, curmode == DeviceOperationModes.Ergo ? curload : curgradient);
                                    if (!SendCommand(curmode))
                                    {
                                        ClosePort();
                                        cmds = 10;
                                        return;
                                    }
                                }
                                else
                                {
                                    cmds++;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ClosePort();
                return;
            }
        }
                
        /* ----------------------------------------------------------------------
         * LOW LEVEL DEVICE IO ROUTINES - PORT TO QIODEVICE REQUIRED BEFORE COMMIT
         *
         *
         * HIGH LEVEL IO
         * int sendCommand()        - writes a command to the device
         * int readMessage()        - reads an inbound message
         *
         * LOW LEVEL IO
         * openPort() - opens serial device and configures it
         * closePort() - closes serial device and releases resources
         * rawRead() - non-blocking read of inbound data
         * rawWrite() - non-blocking write of outbound data
         * discover() - check if a ct is attached to the port specified
         * ---------------------------------------------------------------------- */
        public bool SendCommand(DeviceOperationModes mode)
        {
            switch (mode)
	        {
		        case DeviceOperationModes.Ergo:
                    return RawWrite(ERGO_Command, 56);
                case DeviceOperationModes.SpinScan:
                    return RawWrite(SS_Command, 56);
                default:
                    return false;
	        }
        }

        public int ReadMessage()
        {
            int rc = RawRead(buf, 7);
            if(rc != 0 && (buf[6]&128) == 0)
            {
                // we got something but need to sync
                while((buf[6]&128)==0&&rc>0)
                {
                    rc = RawRead(new byte[] { buf[6] }, 1);
                }
                // at this point we are synced, we may have a dodgy
                // record but that is fair enough if we were out of
                // sync anyway - the alternative is to keep going
                // until we get a good message and that will
                // lead to bigger issues (plus we may have a hw
                // problem anyway).
                //
                // From experience, the need to sync is quite rare
                // on a normally configured and working system
            }
            return rc;
        }

        public bool ClosePort()
        {
            try
            {
                devicePort.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool OpenPort()
        {
            devicePort = new SerialPort(deviceFilename);
            devicePort.ReadTimeout = 1000;
            devicePort.WriteTimeout = 2000;
            devicePort.BaudRate = 2400;
            devicePort.Parity = Parity.None;
            devicePort.StopBits = StopBits.One;
            devicePort.RtsEnable = true;
            devicePort.ParityReplace = 0x0;
            devicePort.Open();
            if(devicePort.IsOpen)
            {
                return true;                                
            }
            else
            {
                return false;
            }
        }

        public bool RawWrite(byte[] bytes, int size)
        {
            try
            {
                devicePort.Write(bytes, 0, size);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int RawRead(byte[] buffer, int size)
        {
            return devicePort.Read(buffer, 0, 7);
        }

        public bool Discover(string device)
        {
            if(string.IsNullOrWhiteSpace(device)) return false;

            byte[] greeting = Encoding.ASCII.GetBytes("Racermate");
            byte[] handshake = new byte[7];

            Device = device;

            OpenPort();

            if(RawWrite(greeting,9))
            {
                if(RawRead(handshake, 6) != 6)
                    return false;
                handshake[6] = Convert.ToByte("\0");
                if(Encoding.ASCII.GetString(handshake).Equals("LinkUp"))
                    return true;
            }
            return false;
        }
    }

    class Telemetry
    {
        public double Power { get; set; }

        public double HeartRate { get; set; }

        public double Cadence { get; set; }

        public double Speed { get; set; }

        public double RRC { get; set; }

        public bool IsCalibrated { get; set; }

        public int Buttons { get; set; }

        public ControlStatuses Status { get; set; }
    }
}

