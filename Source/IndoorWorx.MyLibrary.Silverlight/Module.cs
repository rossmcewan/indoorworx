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
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Events;

namespace IndoorWorx.MyLibrary
{
    public class Module : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        public Module(IUnityContainer unityContainer, IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.unityContainer = unityContainer;
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;

            eventAggregator.GetEvent<MyLibraryEvent>().Subscribe(HandleMyLibraryEvent, true);
            eventAggregator.GetEvent<VideoCreatedEvent>().Subscribe(HandleVideoCreatedEvent, true);
        }

        private void HandleVideoCreatedEvent(Video video)
        {
            var libraryView = serviceLocator.GetInstance<IMyLibraryView>();
            libraryView.NavigateToLibraryPart(LibraryPart.Videos);
            var detailsModel = serviceLocator.GetInstance<IVideoDetailsPresentationModel>();
            detailsModel.SelectVideoWithId(video.Id);            
        }

        private void HandleMyLibraryEvent(LibraryPart libraryPart)
        {
            serviceLocator.GetInstance<INavigationService>().NavigateTo(
                new Uri(
                    string.Format("/MyLibrary?libraryPart={0}", libraryPart.ToString()), 
                    UriKind.Relative));
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

            unityContainer.RegisterInstance<IVideosPresentationModel>(unityContainer.Resolve<VideosPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IVideosView>(unityContainer.Resolve<VideosView>(), new ContainerControlledLifetimeManager());

            unityContainer.RegisterInstance<IVideoDetailsPresentationModel>(unityContainer.Resolve<VideoDetailsPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IVideoDetailsView>(unityContainer.Resolve<VideoDetailsView>(), new ContainerControlledLifetimeManager());

            unityContainer.RegisterInstance<ITemplatesPresentationModel>(unityContainer.Resolve<TemplatesPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<ITemplatesView>(unityContainer.Resolve<TemplatesView>(), new ContainerControlledLifetimeManager());

            unityContainer.RegisterInstance<ITemplateDetailsPresentationModel>(unityContainer.Resolve<TemplateDetailsPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<ITemplateDetailsView>(unityContainer.Resolve<TemplateDetailsView>(), new ContainerControlledLifetimeManager());

            unityContainer.RegisterInstance<IVideoCatalogPresentationModel>(unityContainer.Resolve<VideoCatalogPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IVideoCatalogView>(unityContainer.Resolve<VideoCatalogView>(), new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<ITemplateView, TemplateView>();
            unityContainer.RegisterType<ITemplatePresentationModel, TemplatePresentationModel>();

            unityContainer.RegisterType<IIntervalPresentationModel,IntervalPresentationModel>();
            unityContainer.RegisterType<IIntervalView,IntervalWindow>();

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
        }

        #endregion
    }
}
