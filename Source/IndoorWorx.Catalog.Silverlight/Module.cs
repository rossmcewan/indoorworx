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
using IndoorWorx.Infrastructure.DragDrop;
using IndoorWorx.Library.DragDrop;
using IndoorWorx.Catalog.Resources;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure;

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

        private IDropTargetHost DropTargetHost
        {
            get { return serviceLocator.GetInstance<IDropTargetHost>(); }
        }

        #region IModule Members

        public void Initialize()
        {
            Application.Current.Resources.Add("CatalogResources", new ResourceWrapper());

            unityContainer.RegisterInstance<ICatalogPresentationModel>(unityContainer.Resolve<CatalogPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<ICatalogView>(unityContainer.Resolve<CatalogView>(), new ContainerControlledLifetimeManager());

            unityContainer.RegisterInstance<IVideosPresentationModel>(unityContainer.Resolve<VideosPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IVideosView>(unityContainer.Resolve<VideosView>(), new ContainerControlledLifetimeManager());

            unityContainer.RegisterInstance<IVideoDetailsPresentationModel>(unityContainer.Resolve<VideoDetailsPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IVideoDetailsView>(unityContainer.Resolve<VideoDetailsView>(), new ContainerControlledLifetimeManager());

            unityContainer.RegisterInstance<IVideoCatalogPresentationModel>(unityContainer.Resolve<VideoCatalogPresentationModel>(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance<IVideoCatalogView>(unityContainer.Resolve<VideoCatalogView>(), new ContainerControlledLifetimeManager());


            NavigationLinks.MapUri(
                new Uri("/Catalog", UriKind.Relative),
                new Uri("/IndoorWorx.Catalog.Silverlight;component/Pages/CatalogPage.xaml", UriKind.Relative));

            NavigationLinks.Add(new Infrastructure.Models.NavigationInfo()
            {
                Content = "catalog",
                IsAuthenticationRequired = true,
                NavigationUri = "/Catalog",
                Allow = new string[] { "?" },
                Deny = new string[] { "" }
            });

            var videoDropTarget = new DropTarget(
                payload =>
                {
                    if (payload is Video)
                        ApplicationUser.CurrentUser.Videos.Add(payload as Video);
                },
                payload => payload is Video, () => ApplicationContext.Current.VideoCount)
                {
                    Id = Guid.NewGuid(),
                    Image = new Uri("/IndoorWorx.Catalog.Silverlight;component/Images/library.png", UriKind.Relative),
                    Title = CatalogResources.MyLibraryOfVideos
                };
            ApplicationContext.Current.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "VideoCount")
                        videoDropTarget.ItemCount = ApplicationContext.Current.VideoCount;
                };
            DropTargetHost.AddDropTarget(videoDropTarget);

            DropTargetHost.AddDropTarget(new DropTarget(
                payload =>
                {
                    MessageBox.Show("This should not be called");
                },
                payload => false, () => 0)
                {
                    Id = Guid.NewGuid(),
                    Image = new Uri("/IndoorWorx.Catalog.Silverlight;component/Images/templates.png", UriKind.Relative),
                    Title = CatalogResources.MyLibraryOfTemplates
                });
        }

        #endregion
    }
}
