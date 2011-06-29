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
using IndoorWorx.MyLibrary.Helpers;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.MyLibrary.Views;

namespace IndoorWorx.MyLibrary
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
            Application.Current.Resources.Add("MyLibraryResources", new ResourceWrapper());

            unityContainer.RegisterInstance<IMyLibraryPresentationModel>(unityContainer.Resolve<MyLibraryPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IMyLibraryView>(unityContainer.Resolve<MyLibraryView>(), new ContainerControlledLifetimeManager());

            NavigationLinks.MapUri(
                new Uri("/MyLibrary", UriKind.Relative),
                new Uri("/IndoorWorx.MyLibrary.Silverlight;component/Pages/MyLibraryPage.xaml", UriKind.Relative));

            var link = new Infrastructure.Models.NavigationInfo()
            {
                Content = "mylibrary",
                IsAuthenticationRequired = true,
                NavigationUri = "/MyLibrary",
                Allow = new string[] { "*" },
                Deny = new string[] { "?" }
            };

            NavigationLinks.Add(link);

            //ApplicationUser.CurrentUserChanged += (sender, e) =>
            //    {
            //        if (ApplicationUser.CurrentUser == null)
            //        {
            //            NavigationLinks.Remove(link);
            //        }
            //        else
            //        {
            //            NavigationLinks.Add(link);
            //        }
            //    };
        }

        #endregion
    }
}
