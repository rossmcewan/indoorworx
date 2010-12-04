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
using IndoorWorx.Infrastructure;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Designer.Events;

namespace IndoorWorx.Designer.Domain
{
    public class TrainingSetDesign : BaseModel
    {        
        public TrainingSetDesign()
        {
            AddEntryCommand = new DelegateCommand<object>(AddEntry);
        }

        private void AddEntry(object arg)
        {
            IoC.Resolve<IEventAggregator>().GetEvent<AddDesignEntryEvent>().Publish(this);
        }

        public ICommand AddEntryCommand { get; set; }

        private TrainingSet fromTrainingSet;
        public TrainingSet FromTrainingSet
        {
            get { return fromTrainingSet; }
            set
            {
                fromTrainingSet = value;
                if (value != null)
                    SelectionEnd = value.Duration.TotalSeconds;
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
                if (value >= (SelectionEnd - 10))
                    selectionEnd = value + 10;
                FirePropertyChanged("SelectionStart");
                FirePropertyChanged("SelectionEnd");
                FirePropertyChanged("SelectionDuration");
            }
        }

        public double? SelectionDuration
        {
            get { return SelectionEnd - SelectionStart; }
        }

        private double? selectionEnd;
        public double? SelectionEnd
        {
            get { return selectionEnd; }
            set
            {
                selectionEnd = value;
                if (value <= (SelectionStart + 10))
                    selectionStart = value - 10;
                FirePropertyChanged("SelectionEnd");
                FirePropertyChanged("SelectionStart");
                FirePropertyChanged("SelectionDuration");
            }
        }        
    }
}
