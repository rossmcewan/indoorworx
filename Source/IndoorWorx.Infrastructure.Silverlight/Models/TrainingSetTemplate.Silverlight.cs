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

namespace IndoorWorx.Infrastructure.Models
{
    public partial class TrainingSetTemplate : IEditableObject, IChangeTracking
    {
        public TrainingSetTemplate()
        {
            this.Intervals = new ObservableCollection<Interval>();
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
            SetupIntervalTimes();
        }

        public void SetupIntervalTimes()
        {
            double oaDateTime = 0;
            DateTime today = DateTime.Now;
            foreach (var interval in Intervals)
            {
                interval.OADateTime = oaDateTime;
                var oaToAdd = new DateTime(today.Year, today.Month, today.Day, interval.Duration.Hours, interval.Duration.Minutes, interval.Duration.Seconds).ToOADate();
                oaDateTime += oaToAdd;
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

        public bool ValuesEquals(TrainingSetTemplate compareTo)
        {
            return
                this.Credits == compareTo.Credits &&
                this.Description == compareTo.Description &&
                this.Duration == compareTo.Duration &&
                this.EffortType.Equals(compareTo.EffortType) &&
                this.Title == compareTo.Title &&
                IntervalsAreEqual(this.Intervals, compareTo.Intervals);
        }
    }
}
