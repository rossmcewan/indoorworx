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
    public partial class Interval : IEditableObject, IChangeTracking
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
            set
            {
                this.intervalDuration = value;
                FirePropertyChanged("IntervalDuration");
            }
            get { return intervalDuration; }
        }

        private CountDown toStart = new CountDown();
        public virtual CountDown ToStart
        {
            set
            {
                this.toStart = value;
                FirePropertyChanged("ToStart");
            }
            get { return toStart; }
        }

        private CountDown toEnd = new CountDown();
        public virtual CountDown ToEnd
        {
            set
            {
                this.toEnd = value;
                FirePropertyChanged("ToEnd");
            }
            get { return toEnd; }                
        }

        public static Interval NewWarmupInterval(EffortType effortType, Action onChange)
        {
            return NewInterval(Interval.WarmupTag, effortType, onChange);
        }

        public static Interval NewMainSetInterval(EffortType effortType, Action onChange)
        {
            return NewInterval(string.Empty, effortType, onChange);
        }

        public static Interval NewCooldownInterval(EffortType effortType, Action onChange)
        {
            return NewInterval(Interval.CooldownTag, effortType, onChange);
        }

        private static Interval NewInterval(string tag, EffortType effortType, Action onChange)
        {
            var interval = new Interval();
            interval.Repeats = 1;
            interval.PropertyChanged += (sender, e) => onChange();
            interval.IntervalDuration.PropertyChanged += (sender, e) => onChange();
            interval.RecoveryInterval.PropertyChanged += (sender, e) => onChange();
            interval.ToEnd.PropertyChanged += (sender, e) => onChange();
            interval.ToStart.PropertyChanged += (sender, e) => onChange();
            interval.PropertyChanged += IntervalPropertyChanged;
            interval.IntervalLevel = ApplicationContext.Current.IntervalLevels.FirstOrDefault();
            interval.EffortType = effortType;
            interval.IntervalType = ApplicationContext.Current.IntervalTypes.FirstOrDefault(x => x.Tag == tag);
            return interval;
        }

        private static void IntervalPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var interval = sender as Interval;
            if (args.PropertyName == "IntervalLevel")
            {
                interval.Effort = interval.IntervalLevel.AverageEffortFor(interval.EffortType);
                //if (interval.IntervalLevel != null)
                //{
                //    if (interval.EffortType.IsHR)
                //    {
                //        interval.Effort = (interval.IntervalLevel.MinimumPercentageOfFthr + interval.IntervalLevel.MaximumPercentageOfFthr) / 2;
                //    }
                //    else if (interval.EffortType.IsPower)
                //    {
                //        interval.Effort = (interval.IntervalLevel.MinimumPercentageOfFtp + interval.IntervalLevel.MaximumPercentageOfFtp) / 2;
                //    }
                //    else if (interval.EffortType.IsRPE)
                //    {
                //        interval.Effort = (interval.IntervalLevel.MinRPE + interval.IntervalLevel.MaxRPE) / 2;
                //    }
                //}
            }
            if (args.PropertyName == "EffortType")
            {
                interval.Effort = interval.IntervalLevel.AverageEffortFor(interval.EffortType);
                //if (interval.EffortType != null)
                //{
                //    if (interval.EffortType.IsHR)
                //    {
                //        interval.Effort = (interval.IntervalLevel.MinimumPercentageOfFthr + interval.IntervalLevel.MaximumPercentageOfFthr) / 2;
                //    }
                //    else if (interval.EffortType.IsPower)
                //    {
                //        interval.Effort = (interval.IntervalLevel.MinimumPercentageOfFtp + interval.IntervalLevel.MaximumPercentageOfFtp) / 2;
                //    }
                //    else if (interval.EffortType.IsRPE)
                //    {
                //        interval.Effort = (interval.IntervalLevel.MinRPE + interval.IntervalLevel.MaxRPE) / 2;
                //    }
                //}
            }
        }

        public bool ValuesEquals(Interval compareTo)
        {
            return this.Description == compareTo.Description &&
                this.Duration == compareTo.Duration &&
                this.Effort == compareTo.Effort &&
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

        private Interval backup;
        public void BeginEdit()
        {
            backup = new Interval();
            backup.Description = this.Description;
            backup.Duration = this.Duration;
            backup.Effort = this.Effort;
            backup.EffortType = this.EffortType;
            backup.IntervalLevel = this.IntervalLevel;
            backup.IntervalType = this.IntervalType;
            backup.IntervalDuration = this.IntervalDuration.Clone();
            backup.RecoveryInterval = this.RecoveryInterval.Clone();
            backup.Repeats = this.Repeats;
            backup.Title = this.Title;
            backup.ToEnd = this.ToEnd.Clone();
            backup.ToStart = this.ToStart.Clone();
        }

        public void CancelEdit()
        {
            this.Description = backup.Description;
            this.Duration = backup.Duration;
            this.Effort = backup.Effort;
            this.EffortType = backup.EffortType;
            this.IntervalLevel = backup.IntervalLevel;
            this.IntervalType = backup.IntervalType;
            this.IntervalDuration = backup.IntervalDuration.Clone();
            this.RecoveryInterval = backup.RecoveryInterval.Clone();
            this.Repeats = backup.Repeats;
            this.Title = backup.Title;
            this.ToEnd = backup.ToEnd.Clone();
            this.ToStart = backup.ToStart.Clone();
            backup = null;
        }

        public void EndEdit()
        {
            backup = null;
        }

        public void AcceptChanges()
        {
            backup = null;
        }

        public bool IsChanged
        {
            get 
            {
                return !this.ValuesEquals(backup);
            }
        }
    }
}
