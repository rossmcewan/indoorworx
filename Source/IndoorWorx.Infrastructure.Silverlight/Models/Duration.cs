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
    public class Duration : BaseModel
    {
        private int hours;
        public virtual int Hours
        {
            get { return hours; }
            set
            {
                hours = value;
                FirePropertyChanged("Hours");
            }
        }

        private int minutes;
        public virtual int Minutes
        {
            get { return minutes; }
            set
            {
                minutes = value;
                FirePropertyChanged("Minutes");
            }
        }

        private int seconds;
        public virtual int Seconds
        {
            get { return seconds; }
            set
            {
                seconds = value;
                FirePropertyChanged("Seconds");
            }
        }

        public TimeSpan AsTimeSpan()
        {
            return new TimeSpan(Hours, Minutes, Seconds);
        }

        public override bool Equals(object obj)
        {
            if (obj is Duration)
            {
                return this.AsTimeSpan() == (obj as Duration).AsTimeSpan();
            }
            return base.Equals(obj);
        }
    }
}
