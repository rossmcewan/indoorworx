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
using IndoorWorx.Infrastructure.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IndoorWorx.Infrastructure;

namespace IndoorWorx.Designer.Models
{
    public class TrainingSetDesignEntry : BaseModel
    {
        public event EventHandler IntensityFactorChanged;

        private TrainingSet source;
        public TrainingSet Source
        {
            get { return source; }
            set
            {
                source = value;
                FirePropertyChanged("Source");
                FirePropertyChanged("Name");
            }
        }

        private TimeSpan timeStart;
        public TimeSpan TimeStart
        {
            get { return timeStart; }
            set
            {
                timeStart = value;
                FirePropertyChanged("TimeStart");
                FirePropertyChanged("Name");
            }
        }

        private TimeSpan timeEnd;
        public TimeSpan TimeEnd
        {
            get { return timeEnd; }
            set
            {
                timeEnd = value;
                FirePropertyChanged("TimeEnd");
                FirePropertyChanged("Name");
            }
        }

        private double intensityFactor = 1;
        public double IntensityFactor
        {
            get { return intensityFactor; }
            set
            {
                intensityFactor = value;
                OnIntensityFactorChanged();
                FirePropertyChanged("IntensityFactor");
                FirePropertyChanged("Name");
            }
        }

        private void OnIntensityFactorChanged()
        {
            foreach (var tentry in telemetry)
            {
                tentry.PercentageThreshold *= IntensityFactor;
            }
            if (IntensityFactorChanged != null)
                IntensityFactorChanged(this, EventArgs.Empty);
        }

        private ICollection<Telemetry> telemetry = new ObservableCollection<Telemetry>();
        public ICollection<Telemetry> Telemetry
        {
            get
            {
                GenerateTelemetry();
                return this.telemetry;
            }
        }

        private void GenerateTelemetry()
        {
            telemetry.Clear();
            double seconds = 0;
            var entriesToAdd = Source.Telemetry.Where(x =>
            x.TimePosition.TotalSeconds >= TimeStart.TotalSeconds &&
            x.TimePosition.TotalSeconds <= TimeEnd.TotalSeconds).Select(x =>
            {
                var t = x.Clone();
                t.PercentageThreshold *= IntensityFactor;
                return t;
            }).ToList();
            foreach (var eta in entriesToAdd)
            {
                seconds += this.Source.RecordingInterval;
                eta.TimePosition = TimeSpan.FromSeconds(seconds);
                this.telemetry.Add(eta);
            }
        }

        public string Name
        {
            get
            {
                return string.Format(
                    "{0} from {1} to {2} at {3:P}", 
                    Source.Title,
                    string.Format("{0:N2}:{1:N2}:{2:N2}", TimeStart.Hours, TimeStart.Minutes, TimeStart.Seconds),
                    string.Format("{0:N2}:{1:N2}:{2:N2}", TimeEnd.Hours, TimeEnd.Minutes, TimeEnd.Seconds),
                    IntensityFactor);
            }
        }

        private bool selected;
        public bool IsSelected
        {
            get { return selected; }
            set
            {
                selected = value;
                FirePropertyChanged("IsSelected");
            }
        }
    }
}
