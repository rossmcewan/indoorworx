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

namespace IndoorWorx.Designer.Domain
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
            }
        }
    }
}
