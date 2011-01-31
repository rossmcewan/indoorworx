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
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Designer.Models
{
    public class TrainingSetDesignEntry : BaseModel
    {
        private TrainingSet source;
        public TrainingSet Source
        {
            get { return source; }
            set
            {
                source = value;
                FirePropertyChanged("Source");
                FirePropertyChanged("Name");
            }
        }

        private TimeSpan timeStart;
        public TimeSpan TimeStart
        {
            get { return timeStart; }
            set
            {
                timeStart = value;
                FirePropertyChanged("TimeStart");
                FirePropertyChanged("Name");
            }
        }

        private TimeSpan timeEnd;
        public TimeSpan TimeEnd
        {
            get { return timeEnd; }
            set
            {
                timeEnd = value;
                FirePropertyChanged("TimeEnd");
                FirePropertyChanged("Name");
            }
        }

        private double intensityFactor = 1;
        public double IntensityFactor
        {
            get { return intensityFactor; }
            set
            {
                intensityFactor = value;
                FirePropertyChanged("IntensityFactor");
                FirePropertyChanged("Name");
            }
        }

        public string Name
        {
            get
            {
                return string.Format(
                    "{0} from {1} to {2} at {3:P}", 
                    Source.Description, 
                    TimeStart.ToString("hh:mm:ss"), 
                    TimeEnd.ToString("hh:mm:ss"), 
                    IntensityFactor);
            }
        }
    }
}
