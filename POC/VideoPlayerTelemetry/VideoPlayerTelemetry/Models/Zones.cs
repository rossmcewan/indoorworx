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

namespace VideoPlayerTelemetry.Models
{
    public class Zones : BaseModel
    {
        public Zones()
        {
            this.Zone1 = new Zone() { Colour = Colors.Blue,MinValue = 0, MaxValue = 50 }; 
            
            this.Zone2 = new Zone() { Colour = Colors.Yellow,MinValue = 51, MaxValue = 75 }; 

            this.Zone2 = new Zone() { Colour = Colors.Orange,MinValue = 76, MaxValue = 90 }; 

            this.Zone3 = new Zone() { Colour = Colors.Red,MinValue = 91, MaxValue = 120 }; 
        }

        private Zone zone1;
        public Zone Zone1
        {
            get { return zone1; }
            set
            {
                zone1 = value;
                OnPropertyChanged("Zone1");
            }
        }

        private Zone zone2;
        public Zone Zone2
        {
            get { return zone2; }
            set
            {
                zone2 = value;
                OnPropertyChanged("Zone2");
            }
        }

        private Zone zone3;
        public Zone Zone3
        {
            get { return zone3; }
            set
            {
                zone3 = value;
                OnPropertyChanged("Zone3");
            }
        }

        
        private Zone zone4;
        public Zone Zone4
        {
            get { return zone4; }
            set
            {
                zone4 = value;
                OnPropertyChanged("Zone4");
            }
        }
    }
}
