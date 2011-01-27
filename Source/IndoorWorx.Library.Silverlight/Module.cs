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
using IndoorWorx.Infrastructure.Navigation;
using IndoorWorx.Library.Navigation;
using IndoorWorx.Infrastructure;
using IndoorWorx.Infrastructure.Services;
using IndoorWorx.Library.Services;
using IndoorWorx.Library.Helpers;
using IndoorWorx.Infrastructure.Facades;
using IndoorWorx.Library.Facades;
using Microsoft.Practices.ServiceLocation;

namespace IndoorWorx.Library
{
    public class Module : IModule
    {

        private readonly IUnityContainer unityContainer;
        private readonly IServiceLocator serviceLocator;
        public Module(IUnityContainer unityContainer, IServiceLocator serviceLocator)
        {
            this.unityContainer = unityContainer;
            this.serviceLocator = serviceLocator;
            IoC.Initialize(this.unityContainer);
        }

        #region IModule Members

        public void Initialize()
        {
            Application.Current.Resources.Add("LibraryResources", new ResourceWrapper());
            unityContainer.RegisterInstance<ICache>(unityContainer.Resolve<Cache>());
            unityContainer.RegisterInstance<INavigationService>(unityContainer.Resolve<NavigationService>());
            unityContainer.RegisterInstance<IDialogFacade>(unityContainer.Resolve<DialogFacade>());
            unityContainer.RegisterType<ICategoryService, CategoryService>();
            unityContainer.RegisterType<IApplicationUserService, ApplicationUserService>();
        }

        #endregion


    }
}
