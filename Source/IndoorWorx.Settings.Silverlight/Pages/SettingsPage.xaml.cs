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

namespace IndoorWorx.Settings.Pages
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void CatalogContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            UpdateSelectedLinks(e.Uri);
        }

        private void UpdateSelectedLinks(Uri uri)
        {
            foreach (UIElement child in LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    var uriString = uri.ToString();
                    var compareTo = uriString;
                    if (uriString.Contains('?'))
                        compareTo = uriString.Substring(0, uriString.IndexOf('?'));
                    if (hb.NavigateUri.ToString().Equals(compareTo))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }   
    }
}
