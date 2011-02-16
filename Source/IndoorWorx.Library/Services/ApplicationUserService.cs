﻿using IndoorWorx.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure.Criteria;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Repositories;
using System.ServiceModel.Activation;

namespace IndoorWorx.Library.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IServiceLocator serviceLocator;
        public ApplicationUserService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        #region Repositories

        private IApplicationUserRepository ApplicationUserRepository
        {
            get { return serviceLocator.GetInstance<IApplicationUserRepository>(); }
        }

        private IReferralSourcesRepository ReferralSourcesRepository
        {
            get { return serviceLocator.GetInstance<IReferralSourcesRepository>(); }
        }

        private IOccupationRepository OccupationRepository
        {
            get { return serviceLocator.GetInstance<IOccupationRepository>(); }
        }

        #endregion

        #region Methods

        public ApplicationUser SaveApplicationUser(ApplicationUser user)
        {
            var result = ApplicationUserRepository.Save(user);
            return result;
        }

        public ApplicationUser RetrieveApplicationUser(ApplicationUserFindCriteria criteria)
        {
            var result = ApplicationUserRepository.FindOne(u=> u.Username == criteria.Username);
            return result;
        }

        public ICollection<Occupation> RetrieveAllOccupations()
        {
            var result = OccupationRepository.FindAll(a => a.IsActive == true).ToList();
            return result;
        }

        public ICollection<ReferralSource> RetrieveAllReferralSources()
        {
            var result = ReferralSourcesRepository.FindAll(a => a.IsActive == true);
            return result;
        }
        #endregion
    }
}
