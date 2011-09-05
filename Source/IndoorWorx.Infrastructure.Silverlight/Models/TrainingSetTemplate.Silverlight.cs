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
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Helpers;
using System.Threading;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class TrainingSetTemplate : IEditableObject, IChangeTracking
    {
        public TrainingSetTemplate()
        {
            this.Intervals = new ObservableCollection<Interval>();
        }

        private DateTime startDateTime = DateTimeHelper.ZeroTime;
        public DateTime StartDateTime
        {
            set { this.startDateTime = value; }
            get { return startDateTime; }
        }

        private DateTime endDateTime;
        public DateTime EndDateTime
        {
            set { this.endDateTime = value; }
            get { return endDateTime; }
        }

        private ICollection<IntervalGroup> sets = new ObservableCollection<IntervalGroup>();
        public virtual ICollection<IntervalGroup> Sets
        {
            get { return sets; }
            set
            {
                sets = value;
                FirePropertyChanged("Sets");
            }
        }

        public void ParseSets()
        {
            sets.Clear();
            var warmups = this.Intervals.Where(x => x.TemplateSection == Interval.WarmupTag);
            PopulateIntervals(warmups, x => Sets.Add(x));
            var mainsets = this.Intervals.Where(x => x.TemplateSection == Interval.MainSetTag);
            PopulateIntervals(mainsets, x => Sets.Add(x));            
            var cooldowns = this.Intervals.Where(x => x.TemplateSection == Interval.CooldownTag);
            PopulateIntervals(cooldowns, x => Sets.Add(x));            
        }

        private void PopulateIntervals(IEnumerable<Interval> rootIntervals, Action<IntervalGroup> add)
        {
            var intervalGroups = rootIntervals.GroupBy(x => x.SectionGroup);
            foreach (var group in intervalGroups)
            {
                var intervals = group.ToList();
                var hasRecovery = intervals.Count > 1 && intervals.GroupBy(x => x.Effort).Count() > 1;
                var firstInterval = intervals.First();

                var interval = new IntervalGroup();
                interval.Duration = TimeSpan.FromSeconds(intervals.Sum(x => x.Duration.TotalSeconds));
                interval.Intervals = intervals;                
                interval.TemplateSection = firstInterval.TemplateSection;
                interval.SectionGroup = firstInterval.SectionGroup;
                
                if (hasRecovery)
                {
                    var recoveryInterval = intervals.ElementAt(1);
                    interval.Repeats = intervals.Count / 2;
                    interval.RecoveryInterval = new Infrastructure.Models.Duration() { Hours = recoveryInterval.Duration.Hours, Minutes = recoveryInterval.Duration.Minutes, Seconds = recoveryInterval.Duration.Seconds };
                }
                else
                {
                    interval.RecoveryInterval = new Infrastructure.Models.Duration() { Hours = 0, Minutes = 0, Seconds = 0 };
                    interval.Repeats = intervals.Count;
                }
                add(interval);
            }
        }

        private ICollection<Telemetry> telemetry;
        public virtual ICollection<Telemetry> Telemetry
        {
            get { return telemetry; }
            set
            {
                telemetry = value;
                FirePropertyChanged("Telemetry");
            }
        }

        public event EventHandler TelemetryLoaded;

        private bool telemetryLoading;
        public virtual bool IsTelemetryLoading
        {
            get { return telemetryLoading; }
            set
            {
                telemetryLoading = value;
                FirePropertyChanged("IsTelemetryLoading");
            }
        }

        private bool telemetryLoaded;
        public virtual bool IsTelemetryLoaded
        {
            get { return telemetryLoaded; }
            set
            {
                this.telemetryLoaded = value;
                FirePropertyChanged("IsTelemetryLoaded");
            }
        }

        public void LoadTelemetry()
        {
            if (!IsTelemetryLoaded && !IsTelemetryLoading)
            {
                IsTelemetryLoading = true;
                ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
                    {
                        var _telemetry = new List<Telemetry>();
                        var timer = TimeSpan.Zero;
                        var recordingInterval = TimeSpan.FromSeconds(2);
                        foreach (var interval in Intervals)
                        {
                            var numberOfElements = interval.Duration.TotalSeconds / recordingInterval.TotalSeconds;
                            for (int i = 0; i < numberOfElements; i++)
                            {
                                var telemetry = new Telemetry();
                                telemetry.PercentageThreshold = Convert.ToDouble(interval.Effort);
                                telemetry.TimePosition = timer;
                                _telemetry.Add(telemetry);
                                timer = timer.Add(recordingInterval);
                            }
                        }
                        IsTelemetryLoading = false;
                        Telemetry = _telemetry;
                        IsTelemetryLoaded = true;
                        if (TelemetryLoaded != null)
                            TelemetryLoaded(this, EventArgs.Empty);
                    }));
            }
            else
            {
                if (TelemetryLoaded != null)
                    TelemetryLoaded(this, EventArgs.Empty);
            }
        }

        private Interval selectedInterval;
        public virtual Interval SelectedInterval
        {
            get { return selectedInterval; }
            set
            {
                selectedInterval = value;
                FirePropertyChanged("SelectedInterval");
            }
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (this.Intervals == null)
                this.Intervals = new ObservableCollection<Interval>();
            else
                this.Intervals = new ObservableCollection<Interval>(this.intervals);
            if (this.VideoText == null)
                this.VideoText = new ObservableCollection<VideoText>();
            else
                this.VideoText = new ObservableCollection<VideoText>(this.videoText);
            this.sets = new ObservableCollection<IntervalGroup>();
            StartDateTime = DateTimeHelper.ZeroTime;
            EndDateTime = startDateTime.Add(Duration);
            SetupIntervalTimes();
        }

        public void SetupIntervalTimes()
        {
            double oaDateTime = 0;
            DateTime position = DateTimeHelper.ZeroTime;
            foreach (var interval in Intervals)
            {
                interval.Position = position;
                position = position.Add(interval.Duration);
                interval.OADateTime = oaDateTime;
                var oaToAdd = DateTimeHelper.ZeroTime.Add(interval.Duration).ToOADate(); //new DateTime(zero.Year, zero.Month, zero.Day, interval.Duration.Hours, interval.Duration.Minutes, interval.Duration.Seconds).ToOADate();
                oaDateTime += oaToAdd;
            }
            position = DateTimeHelper.ZeroTime;
            foreach (var group in Sets)
            {
                group.Position = position;
                position = position.Add(group.Duration);
            }
        }

        private TrainingSetTemplate backupValues;
        public void BeginEdit()
        {
            backupValues = new TrainingSetTemplate();
            backupValues.Credits = this.Credits;
            backupValues.Description = this.Description;
            backupValues.Duration = this.Duration;
            backupValues.EffortType = this.EffortType;
            var backupIntervals = new List<Interval>();
            backupIntervals.AddRange(this.Intervals.Select(x => x.Clone()));
            backupValues.Intervals = backupIntervals;
            backupValues.Title = this.Title;
        }

        public void CancelEdit()
        {
            this.Credits = backupValues.Credits;
            this.Description = backupValues.Description;
            this.Duration = backupValues.Duration;
            this.EffortType = backupValues.EffortType;
            this.Intervals.Clear();
            foreach (var interval in backupValues.Intervals)
                this.Intervals.Add(interval);
            this.Title = backupValues.Title;
        }

        public void EndEdit()
        {
            AcceptChanges();
        }

        public void AcceptChanges()
        {
            IsTelemetryLoaded = false;
            backupValues = null;
        }

        public bool IsChanged
        {
            get 
            {
                return !this.ValuesEquals(backupValues);                    
            }
        }

        private bool IntervalsAreEqual(ICollection<Interval> i1, ICollection<Interval> i2)
        {
            if (i1.Count == 0 && i2.Count == 0) return true;
            if (i1.Count != i2.Count) return false;
            for (int i = 0; i < i1.Count; i++)
            {
                var x1 = i1.ElementAt(i);
                var x2 = i2.ElementAt(i);
                if (!x1.ValuesEquals(x2))
                    return false;
            }
            return true;
        }

        private bool VideoTextIsEqual(ICollection<VideoText> v1, ICollection<VideoText> v2)
        {
            if (v1.Count == 0 && v2.Count == 0) return true;
            if (v1.Count != v2.Count) return false;
            for (int i = 0; i < v1.Count; i++)
            {
                var x1 = v1.ElementAt(i);
                var x2 = v2.ElementAt(i);
                if (!x1.ValuesEquals(x2))
                    return false;
            }
            return true;
        }

        public bool ValuesEquals(TrainingSetTemplate compareTo)
        {
            return
                this.Credits == compareTo.Credits &&
                this.Description == compareTo.Description &&
                this.Duration == compareTo.Duration &&
                this.EffortType.Equals(compareTo.EffortType) &&
                this.Title == compareTo.Title &&
                IntervalsAreEqual(this.Intervals, compareTo.Intervals) &&
                VideoTextIsEqual(this.VideoText, compareTo.VideoText);
        }
    }
}
