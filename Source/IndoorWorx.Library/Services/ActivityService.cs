using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Services;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure.Repositories;
using System.ServiceModel.Activation;
using Microsoft.Practices.ServiceLocation;

namespace IndoorWorx.Library.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ActivityService : IActivityService
    {
        private readonly IServiceLocator serviceLocator;
        public ActivityService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        #region Repositories

        private IActivityTypeRepository ActivityTypeRepository
        {
            get { return serviceLocator.GetInstance<IActivityTypeRepository>(); }
        }

        private IEquipmentRepository EquipmentRepository
        {
            get { return serviceLocator.GetInstance<IEquipmentRepository>(); }
        }

        private IEquipmentFeaturesRepository EquipmentFeaturesRepository
        {
            get { return serviceLocator.GetInstance<IEquipmentFeaturesRepository>(); }
        }

        private IManufacturerRepository ManufacturerRepository
        {
            get { return serviceLocator.GetInstance<IManufacturerRepository>(); }
        }
        #endregion

        #region IActivityService Members

        public ICollection<ActivityType> RetrieveAllActivityTypes()
        {
            return ActivityTypeRepository.FindAll(a => a.IsActive == true);
        }

        public ICollection<Equipment> RetrieveAllEquipment()
        {
            return EquipmentRepository.FindAll(a => a.IsActive == true);
        }

        public ICollection<EquipmentFeatures> RetrieveAllEquipmentFeatures()
        {
            return EquipmentFeaturesRepository.FindAll(a => a.IsActive == true);
        }

        public ICollection<Manufacturer> RetrieveAllManufacturers()
        {
            return ManufacturerRepository.FindAll(a => a.IsActive == true);
        }

        #endregion
        
    }
}
