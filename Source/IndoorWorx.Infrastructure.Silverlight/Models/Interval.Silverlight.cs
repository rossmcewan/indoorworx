using System;
using System.Linq;
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
        public static readonly string WarmupTag = "WARMUP";

        public static readonly string CooldownTag = "COOLDOWN";

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

        private Duration recoveryInterval = new Duration();
        public virtual Duration RecoveryInterval
        {
            get { return recoveryInterval; }
            set
            {
                this.recoveryInterval = value;
                FirePropertyChanged("RecoveryInterval");
            }
        }

        private Duration intervalDuration = new Duration();
        public virtual Duration IntervalDuration
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

        public static Interval NewWarmupInterval(EffortType effortType)
        {
            var interval = new Interval();
            interval.EffortType = effortType;
            interval.IntervalType = ApplicationContext.Current.IntervalTypes.FirstOrDefault(x => x.Tag == Interval.WarmupTag);
            interval.IntervalLevel = interval.IntervalType.DefaultLevel;
            if(effortType.IsHR)
            {
                interval.EffortFrom = interval.IntervalLevel.MinimumPercentageOfFthr;
                interval.EffortTo = interval.IntervalLevel.MaximumPercentageOfFthr;
            }
            else if(effortType.IsPower)
            {
                interval.EffortFrom = interval.IntervalLevel.MinimumPercentageOfFtp;
                interval.EffortTo = interval.IntervalLevel.MaximumPercentageOfFtp;
            }
            else if(effortType.IsRPE)
            {
                interval.EffortFrom = interval.IntervalLevel.MinRPE;
                interval.EffortTo = interval.IntervalLevel.MaxRPE;
            }
            return interval;
        }
    }
}
