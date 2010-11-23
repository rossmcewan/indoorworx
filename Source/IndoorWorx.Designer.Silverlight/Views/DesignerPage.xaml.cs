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
using IndoorWorx.Designer.Views;

namespace IndoorWorx.Designer.Views
{
    public partial class DesignerPage : Page
    {
        bool reloadRequired = true;
        public DesignerPage()
        {
            InitializeComponent();
            var contentElement = IoC.Resolve<IDesignerView>() as UserControl;
            if (contentElement.Parent != null)
            {
                (contentElement.Parent as DesignerPage).Content = null;
                reloadRequired = false;
            }
            this.Content = contentElement;
        }

        private IDesignerView View
        {
            get { return this.Content as IDesignerView; }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if (reloadRequired)
                View.Model.LoadCategories();

            string videoId;
            if (this.NavigationContext.QueryString.TryGetValue("VideoId", out videoId))
            {
                View.Model.SelectVideoWithId(new Guid(videoId));
                View.Model.AddDesigner();
            }                   
        }
    }
}
