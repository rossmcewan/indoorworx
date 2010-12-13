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
using IndoorWorx.Designer.Resources;
using IndoorWorx.Infrastructure;
using IndoorWorx.Designer.Helpers;
using IndoorWorx.Designer.Views;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Designer
{
    public class Module : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IServiceLocator serviceLocator;
        public Module(IUnityContainer unityContainer, IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.unityContainer = unityContainer;
            this.serviceLocator = serviceLocator;

            eventAggregator.GetEvent<DesignVideoEvent>().Subscribe(DesignVideo, ThreadOption.UIThread, true);
        }

        public void DesignVideo(Video video)
        {
            NavigationService.NavigateTo(new Uri(string.Format("/Designer?VideoId={0}", video.Id), UriKind.Relative));
        }

        private INavigationService NavigationService
        {
            get { return serviceLocator.GetInstance<INavigationService>(); }
        }

        private INavigationLinks NavigationLinks
        {
            get { return serviceLocator.GetInstance<INavigationLinks>(); }
        }

        #region IModule Members

        public void Initialize()
        {
            Application.Current.Resources.Add("DesignerResources", new ResourceWrapper());

            unityContainer.RegisterInstance<IDesignerPresentationModel>(unityContainer.Resolve<DesignerPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IDesignerView>(unityContainer.Resolve<DesignerView>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IDesignerSelectorPresentationModel, DesignerSelectorPresentationModel>();
            unityContainer.RegisterType<IDesignerSelectorView, DesignerSelectorView>();

            NavigationLinks.MapUri(
                new Uri("/Designer", UriKind.Relative),
                new Uri("/IndoorWorx.Designer.Silverlight;component/Views/DesignerPage.xaml", UriKind.Relative));

            NavigationLinks.Add(new Infrastructure.Models.NavigationInfo()
            {
                Content = "designer",
                IsAuthenticationRequired = true,
                NavigationUri = "/Designer",
                Allow = new string[] { "?" },
                Deny = new string[] { "" }
            });
        }

        #endregion
    }
}
