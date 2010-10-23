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
    public class Zone : BaseModel
    {
        private Color color;
        public Color Colour
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged("Colour");
            }
        }

        private Double minValue;
        public Double MinValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
                OnPropertyChanged("MinValue");
            }
        }

        private Double maxValue;
        public Double MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                OnPropertyChanged("MaxValue");
            }
        }
    }
}
