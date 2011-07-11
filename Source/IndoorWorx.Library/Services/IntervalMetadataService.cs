using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Services;
using System.ServiceModel.Activation;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Repositories;

namespace IndoorWorx.Library.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class IntervalMetadataService : IIntervalMetadataService
    {
        private readonly IServiceLocator serviceLocator;
        public IntervalMetadataService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        private IIntervalLevelRepository IntervalLevelRepository
        {
            get { return serviceLocator.GetInstance<IIntervalLevelRepository>(); }
        }

        private IIntervalTypeRepository IntervalTypeRepository
        {
            get { return serviceLocator.GetInstance<IIntervalTypeRepository>(); }
        }

        private IEffortTypeRepository EffortTypeRepository
        {
            get { return serviceLocator.GetInstance<IEffortTypeRepository>(); }
        }

        public ICollection<Infrastructure.Models.IntervalLevel> FetchIntervalLevels()
        {
            return IntervalLevelRepository.FindAll(null);
        }

        public ICollection<Infrastructure.Models.IntervalType> FetchIntervalTypes()
        {
            return IntervalTypeRepository.FindAll(null);
        }

        public ICollection<Infrastructure.Models.EffortType> FetchEffortTypes()
        {
            return EffortTypeRepository.FindAll(null);
        }
    }
}
