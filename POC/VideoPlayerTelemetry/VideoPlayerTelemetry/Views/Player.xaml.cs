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
using VideoPlayerTelemetry.ViewModels;
using VideoPlayerTelemetry.Library;
using System.Windows.Navigation;

namespace VideoPlayerTelemetry.Views
{
    public partial class Player : UserControl, IPlayer
    {
        public Player()
        {
            InitializeComponent();
            //var lg = new LineGraph() { VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch };
            //Grid.SetColumnSpan(lg, 3);
            //LayoutRoot.Children.Add(lg);
            Ioc.Container.RegisterInstance<IPlayer>(this);
            this.DataContext = Ioc.Container.Resolve<PlayerViewModel>();
        }

        public PlayerViewModel Model
        {
            get { return this.DataContext as PlayerViewModel; }
        }


        public void Play()
        {            
            player.Play();
        }

        public void Stop()
        {
            player.Stop();
        }


        public TimeSpan GetPlayPosition()
        {
            return player.PlaybackPosition;
        }

        private void player_MediaOpened(object sender, EventArgs e)
        {

        }

        private void player_MediaEnded(object sender, EventArgs e)
        {

        }

        private void player_MediaFailed(object sender, Microsoft.SilverlightMediaFramework.Core.CustomEventArgs<Exception> e)
        {

        }

        private void player_LogEntryReceived(object sender, Microsoft.SilverlightMediaFramework.Core.CustomEventArgs<Microsoft.SilverlightMediaFramework.Plugins.Primitives.LogEntry> e)
        {

        }


    }

}
