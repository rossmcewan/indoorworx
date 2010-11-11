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
using Microsoft.Practices.Composite.Events;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Models;
using System.Collections.ObjectModel;

namespace IndoorWorx.Catalog.Services.Mocks
{
    public class MockCategoryService : ICategoryService
    {
        #region ICategoryService Members

        public event EventHandler<DataEventArgs<ICollection<Category>>> CategoriesRetrieved;

        public event EventHandler<DataEventArgs<Exception>> CategoryRetrievalError;

        public void RetrieveCategories()
        {
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
                        if (CategoriesRetrieved != null)
                            CategoriesRetrieved(this, new DataEventArgs<ICollection<Category>>(e.Result));
                    }
                };
            proxy.FindAllAsync();
        }

        #endregion
    }
}
