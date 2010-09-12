using System;
using System.Net;
using RCE.Services.Contracts;
using System.Runtime.Serialization;

namespace RCE.Services.Contracts
{
    [DataContract]
    public class Telemetry : RceObject
    {
        [DataMember]
        public TimeSpan TimePosition { get; set; }

        //Torque - cycling/rowing specific - anything with a ergometer
        [DataMember]
        public decimal Torque { get; set; }

        //Watts
        [DataMember]
        public decimal Watts { get; set; }

        [DataMember]
        public decimal PercentageOfThreshold { get; set; }

        //Speed - in km/h
        [DataMember]
        public decimal Speed { get; set; }

        //Distance - in km
        [DataMember]
        public decimal Distance { get; set; }

        //Cadence
        [DataMember]
        public int Cadence { get; set; }

        //Heart Rate
        [DataMember]
        public int HeartRate { get; set; }

        //Altitude
        [DataMember]
        public int Altitude { get; set; }

        //GPS co-ordinates
        [DataMember]
        public GPSCoordinates GPSCoordinates { get; set; }

        //Gear
        [DataMember]
        public GearRatio GearRatio { get; set; }

        public decimal MaxThreshold
        {
            get { return 100M; }
        }

        public decimal MidThreshold
        {
            get { return 50M; }
        }

        public decimal OverThreshold
        {
            get { return 150M; }
        }

        public Telemetry Clone()
        {
            Telemetry cloned = new Telemetry();
            cloned.Altitude = this.Altitude;
            cloned.Cadence = this.Cadence;
            cloned.Distance = this.Distance;
            cloned.GearRatio = this.GearRatio;
            cloned.GPSCoordinates = this.GPSCoordinates;
            cloned.HeartRate = this.HeartRate;
            cloned.PercentageOfThreshold = this.PercentageOfThreshold;
            cloned.Speed = this.Speed;
            cloned.TimePosition = this.TimePosition;
            cloned.Torque = this.Torque;
            cloned.Watts = this.Watts;
            return cloned;
        }
    }
}
