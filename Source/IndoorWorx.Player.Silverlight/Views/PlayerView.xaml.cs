using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Web.Media.SmoothStreaming;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure;
using IndoorWorx.Infrastructure.Helpers;

namespace IndoorWorx.Player.Views
{
    public partial class PlayerView : UserControl ,IPlayerView
    {
        private readonly IServiceLocator serviceLocator;
        public PlayerView(IPlayerPresentationModel model, IServiceLocator serviceLocator)
        {
            InitializeComponent();            
            this.DataContext = model;
            model.View = this;
            this.serviceLocator = serviceLocator;
        }

        private IShell Shell
        {
            get { return serviceLocator.GetInstance<IShell>(); }
        }


        public IPlayerPresentationModel Model
        {
            get { return this.DataContext as IPlayerPresentationModel; }
        }

        public void LoadVideo(Video video)
        {
            if (video.IsTelemetryLoaded)
                LoadTelemetry(video.Telemetry);
            else
            {
                video.TelemetryLoaded -= video_TelemetryLoaded;
                video.TelemetryLoaded += video_TelemetryLoaded;
                video.LoadTelemetry();
            }
        }

        void video_TelemetryLoaded(object sender, EventArgs e)
        {
            var video = sender as Video;
            LoadTelemetry(video.Telemetry);
        }

        private void LoadTelemetry(ICollection<Telemetry> telemetry)
        {
            SmartDispatcher.BeginInvoke(() =>
                {
                    this.profileChart.LoadTelemetry(telemetry);
                    this.zoomedChart.LoadTelemetry(telemetry);
                });
        }

        private SmoothStreamingMediaElement GetPlayer()
        {
            return mediaElement;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var playStory = this.Resources["playStory"] as Storyboard;
            var player = GetPlayer();
            playStory.Stop();
            Model.Video.IsPlaying = true;
            playStory.Begin();
            player.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            var stopStory = this.Resources["stopStory"] as Storyboard;
            var player = GetPlayer();
            stopStory.Stop();
            Model.Video.IsPlaying = false;
            stopStory.Begin();
            player.Pause();
        }
     
        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            var story = this.Resources["mediaOpenedStory"] as Storyboard;
            story.Stop();
            story.Begin();
            var fe = sender as FrameworkElement;
            var video = fe.DataContext as Video;
            if (video != null)
            {
                video.IsMediaLoading = false;
            }
        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Model.MediaEnded();
        }

        private void fullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
        }

        private void mediaElement_SmoothStreamingErrorOccurred(object sender, SmoothStreamingErrorEventArgs e)
        {

        }
    }
}
