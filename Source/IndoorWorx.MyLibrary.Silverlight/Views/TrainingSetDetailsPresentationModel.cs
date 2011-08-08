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
using IndoorWorx.Infrastructure.Events;

namespace IndoorWorx.MyLibrary.Views
{
    public class TrainingSetDetailsPresentationModel : BaseModel, ITrainingSetDetailPresentationModel
    {
        public event EventHandler TrainingSetRemoved;

        private IEventAggregator eventAggregator;
        private IServiceLocator serviceLocator;
        public TrainingSetDetailsPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            this.PlayTrainingSetCommand = new DelegateCommand<TrainingSet>(PlayTrainingSet);
        }

        private void PlayTrainingSet(TrainingSet video)
        {
            eventAggregator.GetEvent<PlayVideoEvent>().Publish(video);
        }

        private TrainingSet trainingSet;
        public virtual TrainingSet TrainingSet
        {
            get { return trainingSet; }
            set
            {
                trainingSet = value;
                FirePropertyChanged("TrainingSet");
            }
        }
       
        public ICommand PlayTrainingSetCommand { get; private set; }
               
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

        public ITrainingSetDetailsView View { get; set; }

        public void SelectTrainingSetWithId(Guid guid)
        {

        }
    }
}
