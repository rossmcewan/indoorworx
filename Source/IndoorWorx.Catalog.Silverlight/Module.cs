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
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Navigation;
using Microsoft.Practices.Composite.Modularity;
using IndoorWorx.Catalog.Helpers;
using Microsoft.Practices.Unity;
using IndoorWorx.Catalog.Views;
using System.Windows.Navigation;

namespace IndoorWorx.Catalog
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
            Application.Current.Resources.Add("CatalogResources", new ResourceWrapper());

            unityContainer.RegisterInstance<ICatalogPresentationModel>(unityContainer.Resolve<CatalogPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<ICatalogView>(unityContainer.Resolve<CatalogView>(), new ContainerControlledLifetimeManager());


            NavigationLinks.MapUri(
                new Uri("/Catalog", UriKind.Relative),
                new Uri("/IndoorWorx.Catalog.Silverlight;component/Views/CatalogPage.xaml", UriKind.Relative));

            NavigationLinks.Add(new Infrastructure.Models.NavigationInfo()
            {
                Content = "Catalog",
                IsAuthenticationRequired = true,
                NavigationUri = "/Catalog",
                Allow = new string[] { "?" },
                Deny = new string[] { "" }
            });
        }

        #endregion
    }
}
