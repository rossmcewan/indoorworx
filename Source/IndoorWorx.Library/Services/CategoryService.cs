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
    public class CategoryService : ICategoryService
    {
        private readonly IServiceLocator serviceLocator;
        public CategoryService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        private ICategoryRepository CategoryRepository
        {
            get { return serviceLocator.GetInstance<ICategoryRepository>(); }
        }

        #region ICatalogService Members

        public ICollection<Infrastructure.Models.Category> FindAll()
        {
            var result = CategoryRepository.FindAll(null).OrderBy(x=>x.Sequence).ToList();                
            return result;
        }

        #endregion
    }
}
