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

namespace IndoorWorx.Player.Controls
{
    public partial class PlayerControl : UserControl
    {
        public PlayerControl()
        {
            InitializeComponent();
        }

       
        private void videoPlayer_SmoothStreamingErrorOccurred(object sender, Microsoft.Web.Media.SmoothStreaming.SmoothStreamingErrorEventArgs e)
        {

        }

        private void videoPlayer_ManifestReady(object sender, EventArgs e)
        {

        }

        private void videoPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {

        }

        private void videoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {

        }
    }
}
