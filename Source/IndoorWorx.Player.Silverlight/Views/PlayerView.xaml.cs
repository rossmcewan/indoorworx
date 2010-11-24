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
            get { return mediaElement.CurrentState; }
        }

        public void Play()
        {
            var playStory = this.Resources["playStory"] as Storyboard;
            playStory.Stop();
            playStory.Begin();
            mediaElement.Play();
            shouldBePlaying = true;
        }

        public void Pause()
        {
            shouldBePlaying = false;
            var stopStory = this.Resources["stopStory"] as Storyboard;
            stopStory.Stop();
            stopStory.Begin();
            mediaElement.Pause();
        }

        public void LoadTelemetry(ICollection<Telemetry> telemetry)
        {
            SmartDispatcher.BeginInvoke(() =>
                {
                    this.profileChart.LoadTelemetry(telemetry);
                    this.zoomedChart.LoadTelemetry(telemetry);
                });
        }

        bool shouldBePlaying = false;
        
        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            var story = this.Resources["mediaOpenedStory"] as Storyboard;
            story.Stop();
            story.Begin();
            var fe = sender as FrameworkElement;
            Model.MediaOpened();
        }

        public void EndVideo()
        {
            shouldBePlaying = false;
        }

        public void EnsurePlaying()
        {
            if (mediaElement.CurrentState != SmoothStreamingMediaElementState.Playing && shouldBePlaying)
                mediaElement.Play();
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

        private void videoPlayer_ManifestReady(object sender, EventArgs e)
        {
            Model.ManifestReady();            
        }

        private void mediaElement_SmoothStreamingErrorOccurred(object sender, SmoothStreamingErrorEventArgs e)
        {

        }
    }
}
