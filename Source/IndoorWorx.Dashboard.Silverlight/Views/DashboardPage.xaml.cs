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
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Services;
using Microsoft.Practices.Unity;
using IndoorWorx.Infrastructure;

namespace IndoorWorx.Dashboard.Views
{
    public partial class DashboardPage : Page
    {
        private readonly IAuthenticationOperations authOps;
        private readonly IServiceLocator serviceLocator;
        public DashboardPage()
        {
            this.authOps = IoC.Resolve<IAuthenticationOperations>();
            this.serviceLocator = IoC.Resolve<IServiceLocator>();
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //var content = serviceLocator.GetInstance<IDashboardView>();
            //content.Model.Refresh();
            //if (this.Content == null)
            //{
            //    this.Content = content as UIElement;
            //}
            //if (authOps.IsAuthenticated)
            //{
            //    //set the content to the users page
            //    this.Content = serviceLocator.GetInstance<IDashboardView>() as UIElement;
            //}
            //else
            //{
            //    //set the content to an un-authorized page
            //}
        }       
    }
}
