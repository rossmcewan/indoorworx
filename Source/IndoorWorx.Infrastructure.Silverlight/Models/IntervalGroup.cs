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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IndoorWorx.Infrastructure.Models
{
    public class IntervalGroup : Interval
    {
        public override string Title
        {
            set { }
            get
            {
                var hasRecovery = HasRecoveryIntervals;
                var interval = Intervals.First();
                var title = string.Format("{0} x {1} {2}{3}",
                    hasRecovery ? Intervals.Count/2 : Intervals.Count,
                    interval.Duration,
                    interval.IntervalLevel == null ? string.Empty : interval.IntervalLevel.Title,
                    hasRecovery ? string.Format(", {0} RI", Intervals.ElementAt(1).Duration) : string.Empty);
                return title;
            }
        }

        private ICollection<Interval> intervals = new ObservableCollection<Interval>();
        public virtual ICollection<Interval> Intervals
        {
            get { return intervals; }
            set
            {
                intervals = value;
                FirePropertyChanged("Intervals");
            }
        }

        public bool HasRecoveryIntervals
        {
            get
            {
                return intervals.Count > 1 && intervals.GroupBy(x => x.Effort).Count() > 1;
            }
        }

        public override void ClearDesignData()
        {
            base.ClearDesignData();
            foreach (var interval in Intervals)
            {
                interval.ClearDesignData();
            }
        }
    }
}
