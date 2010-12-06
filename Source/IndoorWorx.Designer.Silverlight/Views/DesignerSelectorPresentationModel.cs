using System;
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
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Composite.Presentation.Commands;
using IndoorWorx.Designer.Events;

namespace IndoorWorx.Designer.Views
{
    public class DesignerSelectorPresentationModel : BaseModel, IDesignerSelectorPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        public DesignerSelectorPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            AddEntryCommand = new DelegateCommand<object>(AddEntry);
        }

        private void AddEntry(object arg)
        {
            eventAggregator.GetEvent<AddDesignEntryEvent>().Publish(this);
        }

        public ICommand AddEntryCommand { get; set; }

        private Video source;
        public Video Source
        {
            get { return source; }
            set
            {
                source = value;                
                FirePropertyChanged("Source");
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

        public void OnTrainingSetSelectionChanged()
        {
            SelectionStart = 0;
            SelectionEnd = 10;
        }
    }
}
