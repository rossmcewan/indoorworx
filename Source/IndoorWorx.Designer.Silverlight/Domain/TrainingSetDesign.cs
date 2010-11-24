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
using Microsoft.Practices.Composite.Presentation.Commands;
using System.Collections.Generic;

namespace IndoorWorx.Designer.Domain
{
    public class TrainingSetDesign : BaseModel
    {
        public event EventHandler EntriesChanged;

        public TrainingSetDesign()
        {
            AddToNewTrainingSetCommand = new DelegateCommand<object>(AddToNewTrainingSet);
        }

        private void AddToNewTrainingSet(object arg)
        {
            Entries.Add(new TrainingSetDesignEntry()
            {
                Source = FromTrainingSet,
                TimeStart = TimeSpan.FromSeconds(SelectionStart.GetValueOrDefault()),
                TimeEnd = TimeSpan.FromSeconds(SelectionEnd.GetValueOrDefault())
            });
            if (EntriesChanged != null)
                EntriesChanged(this, EventArgs.Empty);
        }

        public ICommand AddToNewTrainingSetCommand { get; set; }

        private TrainingSet fromTrainingSet;
        public TrainingSet FromTrainingSet
        {
            get { return fromTrainingSet; }
            set
            {
                fromTrainingSet = value;
                FirePropertyChanged("FromTrainingSet");
            }
        }

        private ICollection<TrainingSetDesignEntry> entries = new List<TrainingSetDesignEntry>();
        public ICollection<TrainingSetDesignEntry> Entries
        {
            get { return entries; }
            set
            {
                entries = value;
                FirePropertyChanged("Entries");
            }
        }

        private double? selectionStart;
        public double? SelectionStart
        {
            get { return selectionStart; }
            set
            {
                selectionStart = value;
                FirePropertyChanged("SelectionStart");
            }
        }

        private double? selectionEnd;
        public double? SelectionEnd
        {
            get { return selectionEnd; }
            set
            {
                selectionEnd = value;
                FirePropertyChanged("SelectionEnd");
            }
        }

        internal ICollection<Telemetry> GetDesignedTelemetry()
        {
            List<Telemetry> result = new List<Telemetry>();
            double seconds = 0;
            foreach (var entry in Entries)
            {
                var entriesToAdd = entry.Source.Telemetry.Where(x =>
                        x.TimePosition.TotalSeconds >= entry.TimeStart.TotalSeconds &&
                        x.TimePosition.TotalSeconds <= entry.TimeEnd.TotalSeconds).Select(x => x.Clone()).ToList();
                foreach(var eta in entriesToAdd)
                {
                    eta.TimePosition = TimeSpan.FromSeconds(seconds);
                    seconds += entry.Source.RecordingInterval;
                }
                result.AddRange(entriesToAdd);
            }
            return result;
        }
    }
}
