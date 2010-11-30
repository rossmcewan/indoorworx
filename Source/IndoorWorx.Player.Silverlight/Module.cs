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
using IndoorWorx.Player.Views;
using IndoorWorx.Player.Helpers;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Events;
using Telerik.Windows.Controls;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Practices.Composite.Presentation.Events;
using IndoorWorx.Player.Controls;
using IndoorWorx.Library.Controls;

namespace IndoorWorx.Player
{
    public class Module : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IServiceLocator serviceLocator;
        public Module(IUnityContainer unityContainer, IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.unityContainer = unityContainer;
            this.serviceLocator = serviceLocator;

            eventAggregator.GetEvent<PlayVideoEvent>().Subscribe(PlayVideo, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<PreviewVideoEvent>().Subscribe(PreviewVideo, ThreadOption.UIThread, true);
        }

        public void PlayVideo(Video video)
        {
            RadWindow window = new RadWindow();
            window.WindowState = WindowState.Maximized;
            window.ResizeMode = ResizeMode.NoResize;
            window.Header = video.Title;

            //var player = serviceLocator.GetInstance<IndoorWorx.Player.Views.IPlayerView>();
            //player.Model.Video = video;
            var player = new VideoMediaElement();
            player.Model = video;
            
            window.Content = player;
            window.ShowDialog();
        }

        public void PreviewVideo(Video video)
        {
            RadWindow window = new RadWindow();
            window.WindowStartupLocation = Telerik.Windows.Controls.WindowStartupLocation.CenterScreen;
            window.WindowState = WindowState.Normal;
            window.ResizeMode = ResizeMode.NoResize;
            window.Header = string.Format(Resources.PlayerResources.PreviewVideoTitle, video.Title);
            window.Width = 400;

            var player = new VideoMediaElement();            
            player.Model = video;

            window.Content = player;
            window.ShowDialog();
        }

        private INavigationService NavigationService
        {
            get { return serviceLocator.GetInstance<INavigationService>(); }
        }

        #region IModule Members

        public void Initialize()
        {
            Application.Current.Resources.Add("PlayerResources", new ResourceWrapper());

            unityContainer.RegisterType<IPlayerPresentationModel, PlayerPresentationModel>();
            unityContainer.RegisterType<IPlayerView, PlayerView>();
        }

        #endregion
    }
}
