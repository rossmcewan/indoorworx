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
using Microsoft.Practices.Unity;
using IndoorWorx.Designer.Silverlight.Resources;
using IndoorWorx.Infrastructure;
using IndoorWorx.Designer.Silverlight.Helpers;
using IndoorWorx.Designer.Silverlight.Views;

namespace IndoorWorx.Designer
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

        private INavigationService NavigationService
        {
            get { return serviceLocator.GetInstance<INavigationService>(); }
        }

        #region IModule Members

        public void Initialize()
        {
            Application.Current.Resources.Add("DesignerResources", new ResourceWrapper());

            unityContainer.RegisterType<IDesignerPresentationModel, DesignerPresentationModel>();
            unityContainer.RegisterType<IDesignerView, DesignerView>();
            //unityContainer.RegisterInstance<ICatalogPresentationModel>(unityContainer.Resolve<CatalogPresentationModel>(), new ContainerControlledLifetimeManager());
            //unityContainer.RegisterInstance<ICatalogView>(unityContainer.Resolve<CatalogView>(), new ContainerControlledLifetimeManager());
            
            NavigationService.AddNavigationLink(new Infrastructure.Models.NavigationInfo()
            {
                Content = "Designer",
                IsAuthenticationRequired = true,
                NavigationUri = "/IndoorWorx.Designer.Silverlight;component/Views/Dynamic/DesignerShim.xaml",
                PackageName = "IndoorWorx.Designer.Silverlight.xap",
                Allow = new string[] { "?" },
                Deny = new string[] { "" }
            });
        }

        #endregion
    }
}
