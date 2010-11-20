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

namespace IndoorWorx.Library.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IServiceLocator serviceLocator;
        public CategoryService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public ICache Cache
        {
            get { return serviceLocator.GetInstance<ICache>(); }
        }

        #region ICategoryService Members

        public event EventHandler<DataEventArgs<ICollection<Category>>> CategoriesRetrieved;

        public event EventHandler<DataEventArgs<Exception>> CategoryRetrievalError;

        public void RetrieveCategories()
        {
            var categories = Cache.Get("Categories") as ICollection<Category>;
            if (categories != null)
            {
                if (CategoriesRetrieved != null)
                    CategoriesRetrieved(this, new DataEventArgs<ICollection<Category>>(categories));
            }
            var proxy = new IndoorWorx.Library.CategoryServiceReference.CategoryServiceClient();
            proxy.FindAllCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (CategoryRetrievalError != null)
                            CategoryRetrievalError(this, new DataEventArgs<Exception>(e.Error));
                    }
                    else
                    {
                        Cache.Add("Categories", e.Result, TimeSpan.FromMinutes(10));
                        if (CategoriesRetrieved != null)
                            CategoriesRetrieved(this, new DataEventArgs<ICollection<Category>>(e.Result));
                    }
                };
            proxy.FindAllAsync();
        }

        #endregion
    }
}
