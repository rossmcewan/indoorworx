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
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Navigation;
using IndoorWorx.Dashboard.Views;

namespace IndoorWorx.Dashboard
{
    public class Module : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IServiceLocator serviceLocator;
        public Module(IUnityContainer unityContainer, IServiceLocator serviceLocator)
        {
            this.unityContainer = unityContainer;
            this.serviceLocator = serviceLocator;
        }

        private INavigationLinks NavigationLinks
        {
            get { return serviceLocator.GetInstance<INavigationLinks>(); }
        }
       
        #region IModule Members

        public void Initialize()
        {
            unityContainer.RegisterInstance<IDashboardPresentationModel>(unityContainer.Resolve<DashboardPresentationModel>());
            unityContainer.RegisterInstance<IDashboardView>(unityContainer.Resolve<DashboardView>());

            NavigationLinks.MapUri(
                new Uri("/Dashboard", UriKind.Relative),
                new Uri("/IndoorWorx.Dashboard.Silverlight;component/Views/DashboardPage.xaml", UriKind.Relative));
            
            NavigationLinks.Add(new Infrastructure.Models.NavigationInfo()
            {
                Content = "dashboard",
                IsAuthenticationRequired = true,
                NavigationUri = "/Dashboard",
                Allow = new string[] { "?" },
                Deny = new string[] { "" }
            });            
        }

        #endregion
    }
}
