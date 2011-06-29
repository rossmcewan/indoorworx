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
using IndoorWorx.Infrastructure;

namespace IndoorWorx.Settings.Views
{
    public partial class SettingsView : UserControl, ISettingsView
    {
        private IShell shell;
        public SettingsView(ISettingsPresentationModel model, IShell shell)
        {
            this.shell = shell;
            this.DataContext = model;
            InitializeComponent();
            model.View = this;
        }

        public ISettingsPresentationModel Model
        {
            get { return this.DataContext as ISettingsPresentationModel; }
        }

        public void Show()
        {
            shell.AddToLayoutRoot(this);
        }

        public void Hide()
        {
            shell.RemoveFromLayoutRoot(this);
        }

        private void ContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            UpdateSelectedLinks(e.Uri);
        }

        private void ContentFrame_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {

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
