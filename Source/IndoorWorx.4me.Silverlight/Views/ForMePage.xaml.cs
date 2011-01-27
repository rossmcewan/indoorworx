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

namespace IndoorWorx.ForMe.Views
{
    public partial class ForMePage : Page
    {
     
        bool reloadRequired = true;
        public ForMePage()
        {
            InitializeComponent();
            var contentElement = IoC.Resolve<IForMeView>() as UserControl;
            if (contentElement.Parent != null)
            {
                (contentElement.Parent as ForMePage).Content = null;
                reloadRequired = false;
            }
            this.Content = contentElement;
        }

        private IForMeView View
        {
            get { return this.Content as IForMeView; }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

    }
}
