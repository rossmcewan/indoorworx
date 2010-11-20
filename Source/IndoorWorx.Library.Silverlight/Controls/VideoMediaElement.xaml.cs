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

namespace IndoorWorx.Library.Controls
{
    public partial class VideoMediaElement : UserControl
    {
        public VideoMediaElement()
        {
            InitializeComponent();
        }

        private Video Model
        {
            get { return this.DataContext as Video; }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var playStory = this.Resources["playStory"] as Storyboard;
            var player = GetPlayer();
            playStory.Stop();
            Model.IsPlaying = true;
            playStory.Begin();
            player.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            var stopStory = this.Resources["stopStory"] as Storyboard;
            var player = GetPlayer();
            stopStory.Stop();
            Model.IsPlaying = false;
            stopStory.Begin();
            player.Pause();
        }

        private SmoothStreamingMediaElement GetPlayer()
        {
            return mediaElement;
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
    }
}
