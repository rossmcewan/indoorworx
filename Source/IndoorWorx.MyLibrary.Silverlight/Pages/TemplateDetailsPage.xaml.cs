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

namespace IndoorWorx.MyLibrary.Pages
{
    public partial class TemplateDetailsPage : Page
    {
        bool reloadRequired = true;
        public TemplateDetailsPage()
        {
            InitializeComponent();
            var contentElement = IoC.Resolve<ITemplateDetailsView>() as UserControl;
            if (contentElement.Parent != null)
            {
                (contentElement.Parent as TemplateDetailsPage).Content = null;
                reloadRequired = false;
            }
            this.Content = contentElement;
        }

        private ITemplateDetailsView View
        {
            get { return this.Content as ITemplateDetailsView; }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string templateId;
            if (this.NavigationContext.QueryString.TryGetValue("id", out templateId))
            {
                View.Model.SelectTemplateWithId(new Guid(templateId));
            }
        }

    }
}
