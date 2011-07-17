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

namespace IndoorWorx.Infrastructure.Models
{
    public class CountDown : BaseModel
    {
        private bool active;
        public virtual bool IsActive
        {
            get { return active; }
            set
            {
                active = value;
                FirePropertyChanged("IsActive");
            }
        }

        private Duration duration = new Duration();
        public virtual Duration Duration
        {
            get { return duration; }
        }

        private string startMessage;
        public virtual string StartMessage
        {
            get { return startMessage; }
            set
            {
                startMessage = value;
                FirePropertyChanged("StartMessage");
            }
        }

        private string endMessage;
        public virtual string EndMessage
        {
            get { return endMessage; }
            set
            {
                endMessage = value;
                FirePropertyChanged("EndMessage");
            }
        }

        private Duration tick = new Duration();
        public virtual Duration Tick
        {
            get { return tick; }
        }
    }
}
