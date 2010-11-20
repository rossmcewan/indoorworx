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

namespace IndoorWorx.Player.Views
{
    public partial class PlayerView : UserControl ,IPlayerView
    {
        public PlayerView(IPlayerPresentationModel model)
        {
            InitializeComponent();            
            this.DataContext = model;
            model.View = this;
        }       


        public IPlayerPresentationModel Model
        {
            get { return this.DataContext as IPlayerPresentationModel; }
        }

        public void LoadVideo(Video video)
        {
            this.profileChart.LoadTelemetry(video.Telemetry);
            this.zoomedChart.LoadTelemetry(video.Telemetry);
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

        private void mediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            
        }

        private void mediaElement_SmoothStreamingErrorOccurred(object sender, SmoothStreamingErrorEventArgs e)
        {

        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Model.MediaEnded();
        }


        private void fullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen; 
        }
    
    }
}
