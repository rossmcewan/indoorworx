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
using IndoorWorx.Infrastructure.Models;
using System.Collections.Generic;

namespace IndoorWorx.Catalog.Services
{
    public interface ICategoryService
    {
        event EventHandler<DataEventArgs<ICollection<Category>>> CategoriesRetrieved;

        event EventHandler<DataEventArgs<Exception>> CategoryRetrievalError;

        void RetrieveCategories();
    }
}
