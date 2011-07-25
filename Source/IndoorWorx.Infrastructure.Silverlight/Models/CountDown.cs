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
            set
            {
                this.duration = value;
                FirePropertyChanged("Duration");
            }
            get { return duration; }
        }

        private string startMessage = string.Empty;
        public virtual string StartMessage
        {
            get { return startMessage; }
            set
            {
                startMessage = value;
                FirePropertyChanged("StartMessage");
            }
        }

        private string endMessage = string.Empty;
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
            set
            {
                this.tick = value;
                FirePropertyChanged("Tick");
            }
            get { return tick; }
        }

        public override bool Equals(object obj)
        {
            var cd = obj as CountDown;
            if (cd != null)
            {
                return cd.Duration.Equals(this.Duration) &&
                    cd.EndMessage.Equals(this.EndMessage) &&
                    cd.StartMessage.Equals(this.StartMessage) &&
                    cd.Tick.Equals(this.Tick);
            }
            return base.Equals(obj);
        }

        public CountDown Clone()
        {
            var cloned = (CountDown)this.MemberwiseClone();
            cloned.Tick = this.Tick.Clone();
            cloned.Duration = this.Duration.Clone();
            return cloned;
        }
    }
}
