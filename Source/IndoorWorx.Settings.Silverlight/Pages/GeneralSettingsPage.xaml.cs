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
using IndoorWorx.Settings.Views;

namespace IndoorWorx.Settings.Pages
{
    public partial class GeneralSettingsPage : Page
    {
        public GeneralSettingsPage()
        {
            InitializeComponent();
            var contentElement = IoC.Resolve<IGeneralSettingsView>() as UserControl;
            if (contentElement.Parent != null)
            {
                (contentElement.Parent as GeneralSettingsPage).Content = null;
            }
            this.Content = contentElement;
            (contentElement as IGeneralSettingsView).Model.Initialize();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
