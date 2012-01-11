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
using IndoorWorx.Infrastructure.DragDrop;
using IndoorWorx.Infrastructure;
using IndoorWorx.Catalog.Resources;
using IndoorWorx.Library.DragDrop;
using IndoorWorx.Catalog.Views;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Events;

namespace IndoorWorx.Catalog.Pages
{
    public partial class TemplatesPage : Page
    {
        private bool reloadRequired = true;
        public TemplatesPage()
        {
            InitializeComponent();
            var contentElement = IoC.Resolve<ITemplatesView>() as UserControl;
            if (contentElement.Parent != null)
            {
                (contentElement.Parent as TemplatesPage).Content = null;
                reloadRequired = false;
            }
            this.Content = contentElement;
        }

        private ITemplatesView View
        {
            get { return this.Content as ITemplatesView; }
        }

        // Executes when the user navigates to this page.
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
