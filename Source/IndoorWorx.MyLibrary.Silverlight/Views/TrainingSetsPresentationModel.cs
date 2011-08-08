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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Facades;
using IndoorWorx.Infrastructure;

namespace IndoorWorx.MyLibrary.Views
{
    public class TrainingSetsPresentationModel : BaseModel, ITrainingSetsPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogFacade dialogFacade;

        public TrainingSetsPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator, IDialogFacade dialogFacade)
        {
            ApplicationContext.Current.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "TrainingSetCount")
                    FirePropertyChanged("NumberOfTrainingSetsLabel");
            };
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            this.dialogFacade = dialogFacade;
        }

        public ITrainingSetsView View { get; set; }

        public ICommand AddTrainingSetCommand { get; private set; }

        public void Refresh()
        {
        }

        public string NumberOfTrainingSetsLabel
        {
            get { return string.Format(Resources.MyLibraryResources.NumberOfTrainingSetsLabel, ApplicationContext.Current.TrainingSetCount); }
        }

        private bool busy;
        public virtual bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                FirePropertyChanged("IsBusy");
            }
        }
    }
}
