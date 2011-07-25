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
using System.ComponentModel;

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
            return NewInterval(Interval.WarmupTag, effortType);
        }

        public static Interval NewMainSetInterval(EffortType effortType)
        {
            return NewInterval(string.Empty, effortType);
        }

        public static Interval NewCooldownInterval(EffortType effortType)
        {
            return NewInterval(Interval.CooldownTag, effortType);
        }

        private static Interval NewInterval(string tag, EffortType effortType)
        {
            var interval = new Interval();
            interval.Repeats = 1;
            interval.PropertyChanged += IntervalPropertyChanged;
            interval.IntervalLevel = ApplicationContext.Current.IntervalLevels.FirstOrDefault();
            interval.EffortType = effortType;
            interval.IntervalType = ApplicationContext.Current.IntervalTypes.FirstOrDefault(x => x.Tag == tag);
            //if(interval.IntervalType != null)
            //    interval.IntervalLevel = interval.IntervalType.DefaultLevel;
            return interval;
        }

        private static void IntervalPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var interval = sender as Interval;
            if (args.PropertyName == "IntervalLevel")
            {
                if (interval.IntervalLevel != null)
                {
                    if (interval.EffortType.IsHR)
                    {
                        interval.EffortFrom = interval.IntervalLevel.MinimumPercentageOfFthr;
                        interval.EffortTo = interval.IntervalLevel.MaximumPercentageOfFthr;
                    }
                    else if (interval.EffortType.IsPower)
                    {
                        interval.EffortFrom = interval.IntervalLevel.MinimumPercentageOfFtp;
                        interval.EffortTo = interval.IntervalLevel.MaximumPercentageOfFtp;
                    }
                    else if (interval.EffortType.IsRPE)
                    {
                        interval.EffortFrom = interval.IntervalLevel.MinRPE;
                        interval.EffortTo = interval.IntervalLevel.MaxRPE;
                    }
                }
            }
            //if (args.PropertyName == "IntervalType")
            //{
            //    if (interval.IntervalType != null)
            //        interval.IntervalLevel = interval.IntervalType.DefaultLevel;
            //}
            if (args.PropertyName == "EffortType")
            {
                if (interval.EffortType != null)
                {
                    if (interval.EffortType.IsHR)
                    {
                        interval.EffortFrom = interval.IntervalLevel.MinimumPercentageOfFthr;
                        interval.EffortTo = interval.IntervalLevel.MaximumPercentageOfFthr;
                    }
                    else if (interval.EffortType.IsPower)
                    {
                        interval.EffortFrom = interval.IntervalLevel.MinimumPercentageOfFtp;
                        interval.EffortTo = interval.IntervalLevel.MaximumPercentageOfFtp;
                    }
                    else if (interval.EffortType.IsRPE)
                    {
                        interval.EffortFrom = interval.IntervalLevel.MinRPE;
                        interval.EffortTo = interval.IntervalLevel.MaxRPE;
                    }
                }
            }
        }

        public bool ValuesEquals(Interval compareTo)
        {
            return this.Description == compareTo.Description &&
                this.Duration == compareTo.Duration &&
                this.EffortFrom == compareTo.EffortFrom &&
                this.EffortTo == compareTo.EffortTo &&
                this.EffortType.Equals(compareTo.EffortType) &&
                this.IntervalDuration.Equals(compareTo.IntervalDuration) &&
                this.IntervalLevel.Equals(compareTo.IntervalLevel) &&
                this.IntervalType.Equals(compareTo.IntervalType) &&
                this.RecoveryInterval.Equals(compareTo.RecoveryInterval) &&
                this.Repeats == compareTo.Repeats &&
                this.Title == compareTo.Title &&
                this.ToEnd.Equals(compareTo.ToEnd) &&
                this.ToStart.Equals(compareTo.ToStart);
        }
    }
}
