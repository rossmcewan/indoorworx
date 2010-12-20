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
using IndoorWorx.Player.Animations;
using IndoorWorx.Infrastructure.Models;
using System.Threading;
using IndoorWorx.Infrastructure;
using System.Windows.Threading;

namespace IndoorWorx.Silverlight.Tests
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            LoadTextAnimations();
        }

        private void LoadTextAnimations()
        {
             var text = new VideoText() 
                {                     
                    MainText = "Testing animation",
                    SubText = "Is it working?",
                    Duration = new TimeSpan(0,0,3)
                };

            var animation = new Spinner() 
            { 
               DataContext = text
            };

            Animate(animation, text.Duration);
        }

        private void Animate(FrameworkElement animation, TimeSpan duration)
        {
            Grid.SetColumnSpan(animation, 3);
            Grid.SetRowSpan(animation, 3);
            animation.HorizontalAlignment = HorizontalAlignment.Center;
            animation.VerticalAlignment = VerticalAlignment.Center;
            animation.Visibility = Visibility.Visible;
            LayoutRoot.Children.Add(animation);
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
                        LayoutRoot.Children.Remove(animation);
                    });
                }, animation);
            }            
        }

    }
}
