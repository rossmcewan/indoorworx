namespace IndoorWorx.Silverlight
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using IndoorWorx.Silverlight.LoginUI;
    using IndoorWorx.Infrastructure;
    using IndoorWorx.Infrastructure.Navigation;
    using Microsoft.Practices.Unity;
using IndoorWorx.Infrastructure.Models;
    using IndoorWorx.Silverlight.Controls;
    using System;
    using IndoorWorx.Silverlight.Assets.Resources;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using SLaB.Navigation.ContentLoaders.Auth;
    using SLaB.Navigation.ContentLoaders.Error;
    using System.Windows.Shapes;

    /// <summary>
    /// <see cref="UserControl"/> class providing the main UI for the application.
    /// </summary>
    public partial class Shell : UserControl, IShell, INavigationLinks
    {
        /// <summary>
        /// Creates a new <see cref="MainPage"/> instance.
        /// </summary>
        public Shell(IUnityContainer unityContainer)
        {
            InitializeComponent();
            WebContext.Current.Authentication.LoggedIn += new EventHandler<AuthenticationEventArgs>(Authentication_LoggedIn);
            WebContext.Current.Authentication.LoggedOut += new EventHandler<AuthenticationEventArgs>(Authentication_LoggedOut);
            this.loginContainer.Child = new LoginStatus();
            unityContainer.RegisterInstance<INavigationLinks>(this);
        }

        void Authentication_LoggedOut(object sender, AuthenticationEventArgs e)
        {
            this.ContentFrame.Refresh();
        }

        void Authentication_LoggedIn(object sender, AuthenticationEventArgs e)
        {
            this.ContentFrame.Refresh();
        }        
        
        /// <summary>
        /// After the Frame navigates, ensure the <see cref="HyperlinkButton"/> representing the current page is selected
        /// </summary>
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            foreach (UIElement child in LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
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

        /// <summary>
        /// If an error occurs during navigation, show an error window
        /// </summary>
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ErrorWindow.CreateNew(e.Exception);
        }

        #region IShell Members

        public IShell NavigateTo(System.Uri uri)
        {
            this.ContentFrame.Navigate(uri);
            return this;
        }

        public void Show()
        {
            var busyIndicator = new BusyIndicator();
            busyIndicator.Content = this;
            busyIndicator.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            busyIndicator.VerticalContentAlignment = VerticalAlignment.Stretch;
            
            Application.Current.RootVisual = busyIndicator;
        }

        #endregion

        #region INavigationLinks Members

        private HyperlinkButton currentHyperlink = null;

        public void Add(NavigationInfo navigation)
        {
            var hb = GetHyperlinkButtonFor(navigation);
            if (hb == null)
            {
                hb = navigation.AsHyperlinkButton();
                NavigationAuthRule authRule = new NavigationAuthRule()
                {
                    UriPattern = string.Concat("^", navigation.NavigationUri, "$")
                };
                authRule.Parts.Add(new Deny() { Users = navigation.GetDeniedRolesAsString() });
                authRule.Parts.Add(new Allow() { Users = navigation.GetAllowedRolesAsString() });
                NavigationAuthorizer _authorizer = GetAuthorizer();
                _authorizer.Rules.Add(authRule);
                LinksStackPanel.Children.Add(GetDivider());
                LinksStackPanel.Children.Add(hb);
            }
            else
            {
                throw new System.ArgumentException("Link already exists for NavigationInfo", "navigation");
            }
        }

        private NavigationAuthorizer GetAuthorizer()
        {
            var errorPageLoader = this.ContentFrame.ContentLoader as ErrorPageLoader;
            var authContentLoader = errorPageLoader.ContentLoader as AuthContentLoader;
            return authContentLoader.Authorizer as NavigationAuthorizer;
        }

        private void hb_Click(object sender, RoutedEventArgs e)
        {
            var hb = sender as HyperlinkButton;
            if (hb != null)
            {
                currentHyperlink = hb;
                if (hb.NavigateUri.ToString().Contains(ApplicationUris.AuthorizationError))
                {
                    new LoginRegistrationWindow().Show();
                    return;
                }
            }
        }

        private UIElement GetDivider()
        {
            var rectangle = new Rectangle();
            rectangle.Style = Application.Current.Resources[ResourceDictionaryKeys.DividerStyle] as Style;
            return rectangle;
        }

        private HyperlinkButton GetHyperlinkButtonFor(NavigationInfo navigation)
        {
            return LinksStackPanel.Children.OfType<HyperlinkButton>().FirstOrDefault(x => x.Tag == navigation);
        }

        public bool Remove(NavigationInfo navigation)
        {
            var hb = GetHyperlinkButtonFor(navigation);
            if (hb != null)
            {
                return LinksStackPanel.Children.Remove(hb);
            }
            return false;
        }

        public System.Collections.Generic.IEnumerable<NavigationInfo> All()
        {
            return LinksStackPanel.Children.OfType<HyperlinkButton>().Select(x => x.Tag as NavigationInfo);
        }

        public void Clear()
        {
            LinksStackPanel.Children.Clear();
        }

        #endregion
    }
}