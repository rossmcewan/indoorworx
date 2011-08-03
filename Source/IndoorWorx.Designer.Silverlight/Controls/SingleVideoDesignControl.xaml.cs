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
using IndoorWorx.Designer.Views;

namespace IndoorWorx.Designer.Controls
{
    public partial class SingleVideoDesignControl : UserControl
    {
        public SingleVideoDesignControl()
        {
            InitializeComponent();
        }

        private IDesignerPresentationModelBase Model
        {
            get { return this.DataContext as IDesignerPresentationModelBase; }
        }

        private void RadTimeBar_SelectionChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            SetPositions();
        }

        private void SetPositions()
        {
            try
            {
                //fromMediaElement.Position = this.Model.VideoFrom;
                //toMediaElement.Position = this.Model.VideoTo;
            }
            catch (Exception ex)
            { }
        }

        private void MediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            SetPositions();
            Model.Video.IsMediaLoading = false;
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            SetPositions();
            Model.Video.IsMediaLoading = false;
        }
    }
}
