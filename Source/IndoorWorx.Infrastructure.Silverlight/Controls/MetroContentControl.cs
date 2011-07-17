using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace IndoorWorx.Infrastructure.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class MetroContentControl : ContentControl
    {
        public MetroContentControl()
        {
            this.DefaultStyleKey = typeof(MetroContentControl);

            this.Loaded += MetroContentControl_Loaded;
            this.Unloaded += MetroContentControl_Unloaded;
        }

        private void MetroContentControl_Unloaded(
            object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "AfterUnLoaded", false);
        }

        private void MetroContentControl_Loaded(
            object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "AfterLoaded", true);
        }
    }
}
