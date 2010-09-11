using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RCE.Infrastructure.Models
{
    public class Telemetry : BaseModel
    {
        private TimeSpan timePosition;
        public TimeSpan TimePosition
        {
            get { return timePosition; }
            set
            {
                timePosition = value;
                OnPropertyChanged("TimePosition");
            }
        }

        //Torque - cycling/rowing specific - anything with a ergometer
        public decimal Torque { get; set; }

        //Watts
        public decimal Watts { get; set; }

        //Speed - in km/h
        public decimal Speed { get; set; }

        //Distance - in km
        public decimal Distance { get; set; }

        //Cadence
        public int Cadence { get; set; }

        //Heart Rate
        public int HeartRate { get; set; }

        //Altitude
        public int Altitude { get; set; }

        //GPS co-ordinates
        public GPSCoordinates GPSCoordinates { get; set; }

        //Gear
        public GearRatio GearRatio { get; set; }
    }
}
