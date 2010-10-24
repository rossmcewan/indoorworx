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

namespace IndoorWorx.Catalog.Views
{
    public partial class PreviewWindow : ChildWindow
    {
        public PreviewWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void videoPlayer_ClipError(object sender, Microsoft.Web.Media.SmoothStreaming.ClipEventArgs e)
        {

        }

        private void videoPlayer_SmoothStreamingErrorOccurred(object sender, Microsoft.Web.Media.SmoothStreaming.SmoothStreamingErrorEventArgs e)
        {

        }
    }
}

