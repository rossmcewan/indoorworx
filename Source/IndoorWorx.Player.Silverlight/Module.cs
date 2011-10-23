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
using IndoorWorx.Infrastructure;
using IndoorWorx.Infrastructure.Facades;
using IndoorWorx.Infrastructure.Services;
using IndoorWorx.Player.Resources;

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
            eventAggregator.GetEvent<VideoEndedEvent>().Subscribe(VideoEnded, ThreadOption.UIThread, true);
        }

        private IDialogFacade DialogFacade
        {
            get { return serviceLocator.GetInstance<IDialogFacade>(); }
        }

        private void VideoEnded(Video video)
        {
            DialogFacade.Alert(PlayerResources.ThankYouForUsingIndoorWorx);
        }

        public void PlayVideo(Video video)
        {
            video.PlayFrom = TimeSpan.Zero;
            video.PlayTo = video.Duration;
            video.LoadPlayingTelemetry();
            var capture = unityContainer.Resolve<IPlayerDataCaptureView>();
            capture.Model.Video = video;
            capture.Show(() =>
                {
                    var userService = serviceLocator.GetInstance<IApplicationUserService>();
                    userService.PlayVideoCompleted += (sender, e) =>
                        {
                            switch (e.Value.Status)
                            {
                                case PlayVideoStatus.Success:
                                    video.LoadPlayingTelemetry();
                                    var view = unityContainer.Resolve<IPlayerView>();
                                    view.Model.Video = video;
                                    view.Show();
                                    break;
                                case PlayVideoStatus.InsufficientCredits:
                                    DialogFacade.Alert(PlayerResources.InsufficientCredits);
                                    break;
                                case PlayVideoStatus.Error:
                                    DialogFacade.Alert(e.Value.Message);
                                    break;
                                default:
                                    break;
                            }                            
                        };
                    userService.PlayVideoError += (sender, e) =>
                        {
                            throw e.Value;
                        };
                    userService.PlayVideo(video);
                },
                () =>
                {
                });            
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

        private INavigationLinks NavigationLinks
        {
            get { return serviceLocator.GetInstance<INavigationLinks>(); }
        }

        private INavigationService NavigationService
        {
            get { return serviceLocator.GetInstance<INavigationService>(); }
        }

        private IShell Shell
        {
            get { return serviceLocator.GetInstance<IShell>(); }
        }

        #region IModule Members

        public void Initialize()
        {
            Application.Current.Resources.Add("PlayerResources", new ResourceWrapper());

            unityContainer.RegisterType<IPlayerPresentationModel, PlayerPresentationModel>();
            unityContainer.RegisterType<IPlayerView, PlayerView>();

            unityContainer.RegisterType<IPlayerDataCapturePresentationModel, PlayerDataCapturePresentationModel>();
            unityContainer.RegisterType<IPlayerDataCaptureView, PlayerDataCaptureWindow>();

            NavigationLinks.MapUri(
                new Uri("/Player", UriKind.Relative),
                new Uri("/IndoorWorx.Player.Silverlight;component/Views/PlayerPage.xaml", UriKind.Relative));
        }

        #endregion
    }
}
