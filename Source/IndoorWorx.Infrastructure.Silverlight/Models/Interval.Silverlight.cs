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

        public static readonly string MainSetTag = "MAINSET";

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

        private Video video;
        public virtual Video Video
        {
            get { return video; }
            set
            {
                video = value;
                if (video != null)
                {
                    video.TelemetryLoaded += (sender, e) =>
                        {
                            FirePropertyChanged("VideoFrom");
                            FirePropertyChanged("VideoTo");

                            //VideoFrom = TimeSpan.Zero;
                            //VideoTo = TimeSpan.FromSeconds(Math.Min(video.Duration.TotalSeconds, this.Duration.TotalSeconds));
                        };
                    video.LoadTelemetry();
                }
                FirePropertyChanged("Video");
            }
        }

        private TimeSpan videoFrom;
        public virtual TimeSpan VideoFrom
        {
            get { return videoFrom; }
            set
            {
                videoFrom = value;
                //if (videoTo.Subtract(videoFrom) > duration)
                //    videoFrom = TimeSpan.FromSeconds(Math.Max(0, videoTo.Subtract(duration).TotalSeconds));
                FirePropertyChanged("VideoFrom");
            }
        }

        private TimeSpan videoTo;
        public virtual TimeSpan VideoTo
        {
            get { return videoTo; }
            set
            {
                videoTo = value;
                //if (videoTo.Subtract(videoFrom) > duration)
                //    videoTo = videoFrom.Add(duration);
                FirePropertyChanged("VideoTo");
            }
        }

        public static Interval NewWarmupInterval(EffortType effortType, Action onChange)
        {
            return NewInterval(Interval.WarmupTag, effortType, onChange);
        }

        public static Interval NewMainSetInterval(EffortType effortType, Action onChange)
        {
            return NewInterval(Interval.MainSetTag, effortType, onChange);
        }

        public static Interval NewCooldownInterval(EffortType effortType, Action onChange)
        {
            return NewInterval(Interval.CooldownTag, effortType, onChange);
        }

        public static Interval NewInterval(string tag, EffortType effortType, Action onChange)
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
            interval.TemplateSection = tag;
            interval.SectionGroup = Guid.NewGuid().ToString();
            return interval;
        }        

        private static void IntervalPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var interval = sender as Interval;
            if (args.PropertyName == "IntervalLevel")
            {
                interval.Effort = interval.IntervalLevel.AverageEffortFor(interval.EffortType);
            }
            if (args.PropertyName == "EffortType")
            {
                interval.Effort = interval.IntervalLevel.AverageEffortFor(interval.EffortType);
            }
        }

        public void NotifyOnChange(Action onChange)
        {
            this.PropertyChanged += (sender, e) => onChange();
            this.IntervalDuration.PropertyChanged += (sender, e) => onChange();
            this.RecoveryInterval.PropertyChanged += (sender, e) => onChange();
            this.ToEnd.PropertyChanged += (sender, e) => onChange();
            this.ToStart.PropertyChanged += (sender, e) => onChange();
            this.PropertyChanged += IntervalPropertyChanged;
        }

        public bool ValuesEquals(Interval compareTo)
        {
            return this.Description == compareTo.Description &&
                this.Duration == compareTo.Duration &&
                this.Effort == compareTo.Effort &&
                object.Equals(this.EffortType, compareTo.EffortType) &&
                object.Equals(this.IntervalDuration, compareTo.IntervalDuration) &&
                object.Equals(this.IntervalLevel, compareTo.IntervalLevel) &&
                object.Equals(this.IntervalType, compareTo.IntervalType) &&
                object.Equals(this.RecoveryInterval, compareTo.RecoveryInterval) &&
                this.Repeats == compareTo.Repeats &&
                this.Title == compareTo.Title &&
                object.Equals(this.ToEnd, compareTo.ToEnd) &&
                object.Equals(this.ToStart, compareTo.ToStart);
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
