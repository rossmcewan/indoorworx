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
using IndoorWorx.Infrastructure.Services;
using Microsoft.Practices.Composite.Events;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure;
using IndoorWorx.Library.SportingHabitsServiceReference;

namespace IndoorWorx.Library.Services
{
    public class SportingHabitsService : IndoorWorx.Infrastructure.Services.ISportingHabitsService
    {
        private readonly IServiceLocator serviceLocator;
        public SportingHabitsService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public ICache Cache
        {
            get { return serviceLocator.GetInstance<ICache>(); }
        }

        private SportingHabitsServiceClient proxy = new SportingHabitsServiceClient();
        public SportingHabitsServiceClient Proxy
        {
            get { return proxy; }
        }

        #region TrainingVolumeOptions Methods

        public event EventHandler<DataEventArgs<ICollection<TrainingVolume>>> TrainingVolumeOptionsRetrieved;

        public event EventHandler<DataEventArgs<Exception>> TrainingVolumeOptionsRetrievalError;

        public void RetrieveTrainingVolumeOptions()
        {
            var trainingVolumeOptions = Cache.Get("TrainingVolumeOptions") as ICollection<TrainingVolume>;
            if (trainingVolumeOptions != null)
            {
                if (TrainingVolumeOptionsRetrieved != null)
                    TrainingVolumeOptionsRetrieved(this, new DataEventArgs<ICollection<TrainingVolume>>(trainingVolumeOptions));
            }
            else
            {
                Proxy.RetrieveTrainingVolumeOptionsCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (TrainingVolumeOptionsRetrievalError != null)
                            TrainingVolumeOptionsRetrievalError(this, new DataEventArgs<Exception>(e.Error));
                    }
                    else
                    {
                        if (TrainingVolumeOptionsRetrieved != null)
                            TrainingVolumeOptionsRetrieved(this, new DataEventArgs<ICollection<TrainingVolume>>(e.Result));
                    }
                };
                proxy.RetrieveTrainingVolumeOptionsAsync();
            }
        }

        #endregion

        #region IndoorTrainingFrequency Methods

        public event EventHandler<DataEventArgs<ICollection<IndoorTrainingFrequency>>> IndoorTrainingFrequencyOptionsRetrieved;

        public event EventHandler<DataEventArgs<Exception>> IndoorTrainingFrequencyOptionsRetrievalError;

        public void RetrieveIndoorTrainingFrequencyOptions()
        {
            var indoorTrainingFrequencyOptions = Cache.Get("IndoorTrainingFrequencyOptions") as ICollection<IndoorTrainingFrequency>;
            if (indoorTrainingFrequencyOptions != null)
            {
                if (IndoorTrainingFrequencyOptionsRetrieved != null)
                    IndoorTrainingFrequencyOptionsRetrieved(this, new DataEventArgs<ICollection<IndoorTrainingFrequency>>(indoorTrainingFrequencyOptions));
            }
            else
            {
                Proxy.RetrieveIndoorTrainingFrequencyCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (IndoorTrainingFrequencyOptionsRetrievalError != null)
                            IndoorTrainingFrequencyOptionsRetrievalError(this, new DataEventArgs<Exception>(e.Error));
                    }
                    else
                    {
                        if (IndoorTrainingFrequencyOptionsRetrieved != null)
                            IndoorTrainingFrequencyOptionsRetrieved(this, new DataEventArgs<ICollection<IndoorTrainingFrequency>>(e.Result));
                    }
                };
                proxy.RetrieveIndoorTrainingFrequencyAsync();
            }
        }

        #endregion

        #region CompetitiveLevels Methods

        public event EventHandler<DataEventArgs<ICollection<CompetitiveLevel>>> CompetitiveLevelsRetrieved;

        public event EventHandler<DataEventArgs<Exception>> CompetitiveLevelsRetrievalError;

        public void RetrieveCompetitiveLevels()
        {
            var competitiveLevels = Cache.Get("CompetitiveLevels") as ICollection<CompetitiveLevel>;
            if (competitiveLevels != null)
            {
                if (CompetitiveLevelsRetrieved != null)
                    CompetitiveLevelsRetrieved(this, new DataEventArgs<ICollection<CompetitiveLevel>>(competitiveLevels));
            }
            else
            {
                Proxy.RetrieveCompetitiveLevelsCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (CompetitiveLevelsRetrievalError != null)
                            CompetitiveLevelsRetrievalError(this, new DataEventArgs<Exception>(e.Error));
                    }
                    else
                    {
                        if (CompetitiveLevelsRetrieved != null)
                            CompetitiveLevelsRetrieved(this, new DataEventArgs<ICollection<CompetitiveLevel>>(e.Result));
                    }
                };
                proxy.RetrieveTrainingVolumeOptionsAsync();
            }
        }

        #endregion
    }
}
