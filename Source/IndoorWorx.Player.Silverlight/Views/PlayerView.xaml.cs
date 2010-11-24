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
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Telerik.Windows.Controls;

namespace IndoorWorx.Player.Views
{
    public partial class PlayerView : UserControl ,IPlayerView
    {
        private DateTime now = DateTime.Now;
        private double zoomedLength;

        public PlayerView(IPlayerPresentationModel model, IServiceLocator serviceLocator)
        {
            InitializeComponent();            
            this.DataContext = model;
            model.View = this;
        }
        public IPlayerPresentationModel Model
        {
            get { return this.DataContext as IPlayerPresentationModel; }
        }

        public SmoothStreamingMediaElementState CurrentPlayerState
        {
            get { return GetPlayer().CurrentState; }
        }

        public void Play()
        {
            GetPlayer().Play();
        }                

        public void LoadTelemetry(ICollection<Telemetry> telemetry)
        {
            SmartDispatcher.BeginInvoke(() =>
                {
                    this.profileChart.LoadTelemetry(telemetry);
                    this.zoomedChart.LoadTelemetry(telemetry);
                });
        }

        public SmoothStreamingMediaElement GetPlayer()
        {
            return mediaElement;
        }

        bool shouldBePlaying = false;
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var playStory = this.Resources["playStory"] as Storyboard;
            var player = GetPlayer();
            playStory.Stop();
            Model.Video.IsPlaying = true;
            playStory.Begin();
            player.Play();
            shouldBePlaying = true;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            shouldBePlaying = false;
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
            Model.MediaOpened();
        }

        public void EndVideo()
        {
            shouldBePlaying = false;
        }

        public void EnsurePlaying()
        {
            if (GetPlayer().CurrentState != SmoothStreamingMediaElementState.Playing && shouldBePlaying)
                GetPlayer().Play();
        }

        private bool ended = false;
        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.ended = true;
        }

        private void fullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
        }

        public void UpdateZoom(TimeSpan PlayerPosition)
        {
            var xpos = new DateTime(now.Year, now.Month, now.Day, PlayerPosition.Hours, PlayerPosition.Minutes, PlayerPosition.Seconds).ToOADate();
            var rangeFrom = PlayerPosition.TotalSeconds / Model.Video.Duration.TotalSeconds;
            var rangeTo = rangeFrom + zoomedLength;
            this.zoomedChart.SetZoomScrollSettings(rangeFrom, rangeTo);
        }

       
        private void videoPlayer_ManifestReady(object sender, EventArgs e)
        {
            zoomedLength = TimeSpan.FromMinutes(3).TotalSeconds / Model.Video.Duration.TotalSeconds;
            this.zoomedChart.SetZoomScrollSettings(0, zoomedLength);
        }

        private void mediaElement_SmoothStreamingErrorOccurred(object sender, SmoothStreamingErrorEventArgs e)
        {

        }
    }
}
