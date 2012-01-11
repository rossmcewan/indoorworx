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
    public partial class VideoCatalogPage : Page
    {
        private bool reloadRequired = true;
        public VideoCatalogPage()
        {
            InitializeComponent();
            var contentElement = IoC.Resolve<IVideoCatalogView>() as UserControl;
            if (contentElement.Parent != null)
            {
                (contentElement.Parent as VideoCatalogPage).Content = null;
                reloadRequired = true;
            }
            this.Content = contentElement;
        }

        private IVideoCatalogView View
        {
            get
            {
                return this.Content as IVideoCatalogView;
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (reloadRequired)
                View.Model.Refresh();
            string filter = "ALL";
            this.NavigationContext.QueryString.TryGetValue("filter", out filter);
            string orderBy = "CATEGORY";
            this.NavigationContext.QueryString.TryGetValue("orderBy", out orderBy);
            View.Model.FilterVideosBy(filter).OrderVideosBy(orderBy);
            IoC.Resolve<IEventAggregator>().GetEvent<PageLoadedEvent>().Publish(null);
        }
    }
}
