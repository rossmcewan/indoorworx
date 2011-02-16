using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Services;
using System.ServiceModel.Activation;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Repositories;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Library.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SocialMediaService : ISocialMediaService
    {
        private readonly IServiceLocator serviceLocator;
        public SocialMediaService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        #region Repositories

        private ISocialMediaNotificationRepository SocialMediaNotificationRepository
        {
            get { return serviceLocator.GetInstance<ISocialMediaNotificationRepository>(); }
        }

        private ISocialMediaTypeRepository SocialMediaTypeRepository
        {
            get { return serviceLocator.GetInstance<ISocialMediaTypeRepository>(); }
        }

        #endregion

        public ICollection<SocialMediaType> RetrieveSocialMediaTypes()
        {
            return SocialMediaTypeRepository.FindAll(a => a.IsActive == true);
        }

        public ICollection<SocialMediaNotification> SocialMediaNotificationOptions()
        {
            return SocialMediaNotificationRepository.FindAll(a => a.IsActive == true);
        }
    }
}
