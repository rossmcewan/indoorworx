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
    public partial class Interval
    {
        private double oaDateTime;
        public virtual double OADateTime
        {
            get { return oaDateTime; }
            set
            {
                oaDateTime = value;
                FirePropertyChanged("OADateTime");
            }
        }

        public int AverageEffort
        {
            get 
            { 
                var sum = this.effortFrom.GetValueOrDefault() + this.effortTo.GetValueOrDefault();
                var ave = sum / 2;
                return ave; 
            }
        }

        private int repeats;
        public virtual int Repeats
        {
            get { return repeats; }
            set
            {
                repeats = value;
                FirePropertyChanged("Repeats");
            }
        }

        private TimeSpan recoveryInterval;// = new Duration();
        public virtual TimeSpan RecoveryInterval
        {
            get { return recoveryInterval; }
            set
            {
                this.recoveryInterval = value;
                FirePropertyChanged("RecoveryInterval");
            }
        }

        private TimeSpan intervalDuration;// = new Duration();
        public virtual TimeSpan IntervalDuration
        {
            get { return intervalDuration; }
        }

        private CountDown toStart = new CountDown();
        public virtual CountDown ToStart
        {
            get { return toStart; }
        }

        private CountDown toEnd = new CountDown();
        public virtual CountDown ToEnd
        {
            get { return toEnd; }                
        }
    }
}
