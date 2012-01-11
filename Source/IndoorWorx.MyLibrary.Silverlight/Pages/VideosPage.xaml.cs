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
using System.Windows.Navigation;
using IndoorWorx.Infrastructure;
using IndoorWorx.MyLibrary.Views;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Events;

namespace IndoorWorx.MyLibrary.Pages
{
    public partial class VideosPage : Page
    {
        bool reloadRequired = true;
        public VideosPage()
        {
            InitializeComponent();
            var contentElement = IoC.Resolve<IVideosView>() as UserControl;
            if (contentElement.Parent != null)
            {
                (contentElement.Parent as VideosPage).Content = null;
                reloadRequired = false;
            }
            this.Content = contentElement;
        }

        private IVideosView View
        {
            get { return this.Content as IVideosView; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (reloadRequired)
                View.Model.Refresh();
            IoC.Resolve<IEventAggregator>().GetEvent<PageLoadedEvent>().Publish(null);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
    }
}
