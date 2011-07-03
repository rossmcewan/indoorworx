using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Repositories;
using System.ServiceModel.Activation;

namespace IndoorWorx.Library.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TrainingSetTemplateService : ITrainingSetTemplateService
    {
        private readonly IServiceLocator serviceLocator;
        public TrainingSetTemplateService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public ITrainingSetTemplateRepository Repository
        {
            get { return serviceLocator.GetInstance<ITrainingSetTemplateRepository>(); }
        }

        public ICollection<Infrastructure.Models.TrainingSetTemplate> FindAll()
        {
            return Repository.FindAll(null);
        }
    }
}
