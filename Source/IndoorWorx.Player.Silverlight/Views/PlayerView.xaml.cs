﻿using System;
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
    public partial class PlayerView : UserControl, IPlayerView
    {
        private DateTime now = DateTime.Now;
        private IServiceLocator serviceLocator;

        public PlayerView(IPlayerPresentationModel model, IServiceLocator serviceLocator)
        {
            InitializeComponent();
            this.serviceLocator = serviceLocator;
            this.DataContext = model;
            model.View = this;
        }

        private IShell Shell
        {
            get { return serviceLocator.GetInstance<IShell>(); }
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
            mediaElement.Play();           
            shouldBePlaying = true;
        }

        public void Stop()
        {
            shouldBePlaying = false;
            if(mediaElement.SmoothStreamingSource != null)
                mediaElement.Stop();
        }

        public void Pause()
        {
            shouldBePlaying = false;
            if(mediaElement.SmoothStreamingSource != null)
                mediaElement.Pause();
        }

        public void LoadTelemetry(ICollection<Telemetry> telemetry)
        {
            SmartDispatcher.BeginInvoke(() =>
                {
                    this.profileChart.LoadTelemetry(telemetry);
                });
        }

        bool shouldBePlaying = false;
        
        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
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
            Model.MediaError(e);
        }

        public void Show()
        {
            Shell.AddToLayoutRoot(this);
        }

        public void Hide()
        {
            Shell.IsFullScreen = false;
            Shell.RemoveFromLayoutRoot(this);
        }

        public void AddTextAnimation(VideoText videoText)
        {
            FrameworkElement animation = null;
            switch (videoText.Animation)
            {
                case IndoorWorx.Infrastructure.Enums.VideoTextAnimations.FadeCenter:
                     animation = new Fade() { DataContext = videoText };
                    break;
                case IndoorWorx.Infrastructure.Enums.VideoTextAnimations.ZoomCenter:
                     animation = new Zoom() { DataContext = videoText };
                    break;
                case IndoorWorx.Infrastructure.Enums.VideoTextAnimations.ScrollingCenter:
                     animation = new Scrolling() { DataContext = videoText };
                    break;
                case IndoorWorx.Infrastructure.Enums.VideoTextAnimations.Spinner:
                     animation = new Spinner() { DataContext = videoText };
                    break;
                default:
                    break;
            }
            if (animation != null)
            {
                LoadTextAnimation(animation, videoText.Duration);
            }
        }


        private void LoadTextAnimation(FrameworkElement animation,TimeSpan duration)
        {
            Grid.SetColumnSpan(animation, 3);
            Grid.SetRowSpan(animation, 3);
            animation.HorizontalAlignment = HorizontalAlignment.Center;
            animation.VerticalAlignment = VerticalAlignment.Center;
            animation.Visibility = Visibility.Visible;
            playerGrid.Children.Add(animation);
            var startAnimation = animation.Resources["InTransition"] as Storyboard;
            if (startAnimation != null)
            {
                startAnimation.Begin();
                ThreadPool.QueueUserWorkItem((_animation) =>
                {
                    Thread.Sleep(Convert.ToInt32(duration.TotalMilliseconds));
                    SmartDispatcher.BeginInvoke(() =>
                    {
                        var stopAnimation = animation.Resources["OutTransition"] as Storyboard;
                        stopAnimation.Begin();
                        playerGrid.Children.Remove(animation);
                    });
                }, animation);
            }
        }

        public void SetStartPosition(TimeSpan timeSpan)
        {
            mediaElement.Position = timeSpan;
        }

        private void mediaElement_ClipError(object sender, ClipEventArgs e)
        {

        }

        public void Cleanup()
        {
            mediaElement.Dispose();
        }
    }
}
