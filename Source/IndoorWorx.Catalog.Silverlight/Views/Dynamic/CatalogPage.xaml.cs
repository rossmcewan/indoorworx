using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using DynamicNavigation;
using IndoorWorx.Infrastructure;

namespace IndoorWorx.Catalog.Views.Dynamic
{
    public partial class CatalogPage : DynamicPage
    {
        public CatalogPage()
        {
            InitializeComponent();
            Content.Content = IoC.Resolve<ICatalogView>();
        }

        private ICatalogView View
        {
            get { return Content.Content as ICatalogView; }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            View.Model.LoadCategories();
        }
    }
}
