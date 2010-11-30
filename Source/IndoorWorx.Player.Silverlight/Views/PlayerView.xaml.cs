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
using IndoorWorx.Player.Animations;
using System.Threading;

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

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Model.MediaEnded();
        }

        private void videoPlayer_ManifestReady(object sender, EventArgs e)
        {
            Model.ManifestReady();            
        }

        private void mediaElement_SmoothStreamingErrorOccurred(object sender, SmoothStreamingErrorEventArgs e)
        {

        }

        public void AddTextAnimation(VideoText videoText)
        {
            switch (videoText.Animation)
            {
                case IndoorWorx.Infrastructure.Enums.VideoTextAnimations.FadeCenter:
                    var animation = new FadeCenter() { DataContext = videoText };
                    Grid.SetColumnSpan(animation, 3);
                    Grid.SetRowSpan(animation, 3);
                    animation.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    animation.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    playerGrid.Children.Add(animation);
                    var startAnimation = animation.Resources["InTransition"] as Storyboard;
                    startAnimation.Begin();
                    ThreadPool.QueueUserWorkItem((_animation) =>
                        {
                            Thread.Sleep(Convert.ToInt32(videoText.Duration.TotalMilliseconds));
                            SmartDispatcher.BeginInvoke(() =>
                                {
                                    var stopAnimation = animation.Resources["OutTransition"] as Storyboard;
                                    stopAnimation.Begin();
                                    playerGrid.Children.Remove(animation);
                                });
                        }, animation);
                    break;
                case IndoorWorx.Infrastructure.Enums.VideoTextAnimations.ZoomCenter:
                    break;
                case IndoorWorx.Infrastructure.Enums.VideoTextAnimations.ScrollingCenter:
                    break;
                case IndoorWorx.Infrastructure.Enums.VideoTextAnimations.Spinner:
                    break;
                default:
                    break;
            }
        }
    }
}
