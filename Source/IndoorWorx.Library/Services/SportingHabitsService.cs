using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Services;
using System.ServiceModel.Activation;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Repositories;

namespace IndoorWorx.Library.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SportingHabitsService : ISportingHabitsService
    {
        private readonly IServiceLocator serviceLocator;
        public SportingHabitsService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        #region Repositories

        private ITrainingVolumeRepository TrainingVolumeRepository
        {
            get { return serviceLocator.GetInstance<ITrainingVolumeRepository>(); }
        }

        private IIndoorTrainingFrequencyRepository IndoorTrainingFrequencyRepository
        {
            get { return serviceLocator.GetInstance<IIndoorTrainingFrequencyRepository>(); }
        }

        private ICompetitiveLevelRepository CompetitiveLevelRepository
        {
            get { return serviceLocator.GetInstance<ICompetitiveLevelRepository>(); }
        }

        #endregion

        #region Methods

        public ICollection<TrainingVolume> RetrieveTrainingVolumeOptions()
        {
            var result = TrainingVolumeRepository.FindAll(a => a.IsActive == true);
            return result;
        }

        public ICollection<IndoorTrainingFrequency> RetrieveIndoorTrainingFrequency()
        {
            var result = IndoorTrainingFrequencyRepository.FindAll(a => a.IsActive == true);
            return result;
        }

        public ICollection<CompetitiveLevel> RetrieveCompetitiveLevels()
        {
            var result = CompetitiveLevelRepository.FindAll(a => a.IsActive == true);
            return result;
        }

        #endregion
    }
}
