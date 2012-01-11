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
    using System.Windows.Media.Animation;
    using IndoorWorx.Infrastructure.Animations;
    using IndoorWorx.Infrastructure.DragDrop;
    using Telerik.Windows.Controls.DragDrop;
    using Microsoft.Practices.Composite.Events;
    using IndoorWorx.Infrastructure.Events;
    using System.Windows.Input;
    using IndoorWorx.Infrastructure.Behaviors;

    /// <summary>
    /// <see cref="UserControl"/> class providing the main UI for the application.
    /// </summary>
    public partial class Shell : UserControl, IShell, INavigationLinks
    {
        private IUnityContainer container;
        /// <summary>
        /// Creates a new <see cref="MainPage"/> instance.
        /// </summary>
        public Shell(IUnityContainer unityContainer)
        {
            this.container = unityContainer;
            InitializeComponent();

            WebContext.Current.Authentication.LoggedIn += new EventHandler<AuthenticationEventArgs>(Authentication_LoggedIn);
            WebContext.Current.Authentication.LoggedOut += new EventHandler<AuthenticationEventArgs>(Authentication_LoggedOut);
            this.loginContainer.Child = new LoginStatus();
            unityContainer.RegisterInstance<INavigationLinks>(this);
        }

        private void PageLoaded(object arg)
        {
            searchBox.Text = "";
        }

        #region Borderless behaviors

        const int MIN_WIDTH = 800;
        const int MIN_HEIGHT = 600;

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Maximized;

            // Toggle between restore and maximize buttons
            restoreButton.Visibility = System.Windows.Visibility.Visible;
            maximizeButton.Visibility = System.Windows.Visibility.Collapsed;

            // Don't show the resize icon if we're maximized
            resizeButton.Visibility = System.Windows.Visibility.Collapsed;

            // Restore to it's original opacity since this won't be reset with the MouseLeave Event
            maximizeButton.Opacity = 0.5;

        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
        }

        private void restoreButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Normal;

            maximizeButton.Visibility = System.Windows.Visibility.Visible;
            restoreButton.Visibility = System.Windows.Visibility.Collapsed;

            // Make sure the resize icon is showing 
            resizeButton.Visibility = System.Windows.Visibility.Visible;

            // Restore to it's original opacity since this won't be reset with the MouseLeave Event
            restoreButton.Opacity = 0.5;
        }

        private void Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // If the user initiates a drag in the top part of the screen then start moving the window.
            if (e.GetPosition(this).Y < 50.0)
                App.Current.MainWindow.DragMove();
        }

        private void HyperLinkButton_MouseEnter(object sender, MouseEventArgs e)
        {
            HyperlinkButton button = sender as HyperlinkButton;
            button.Opacity = 1;
        }

        private void HyperLinkButton_MouseLeave(object sender, MouseEventArgs e)
        {
            HyperlinkButton button = sender as HyperlinkButton;
            button.Opacity = 0.5;
        }

        private void resizeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragResize(WindowResizeEdge.BottomRight);
        }

        private void border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement border = sender as FrameworkElement;
            App.Current.MainWindow.DragResize((WindowResizeEdge)Enum.Parse(typeof(WindowResizeEdge), border.Tag.ToString(), true));
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < MIN_WIDTH)
            {
                App.Current.MainWindow.Width = MIN_WIDTH;
            }
            if (e.NewSize.Height < MIN_HEIGHT)
            {
                App.Current.MainWindow.Height = MIN_HEIGHT;
            }
        }

        #endregion

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
                    var mappedUri = (this.ContentFrame.UriMapper as UriMapper).UriMappings.FirstOrDefault(x => x.Uri.OriginalString.Equals(hb.NavigateUri));
                    
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

        /// <summary>
        /// If an error occurs during navigation, show an error window
        /// </summary>
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ErrorWindow.CreateNew(e.Exception);
        }

        #region IShell Members

        public void Show()
        {
            container.Resolve<IEventAggregator>().GetEvent<PageLoadedEvent>().Subscribe(PageLoaded, true);

            var busyIndicator = new BusyIndicator();
            busyIndicator.Content = this;
            busyIndicator.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            busyIndicator.VerticalContentAlignment = VerticalAlignment.Stretch;

            (Application.Current as App).RootContainer.SwitchControl(busyIndicator);
        }

        public void AddToLayoutRoot(UIElement ui)
        {
            LayoutRoot.Children.Add(ui);
        }

        public void RemoveFromLayoutRoot(UIElement ui)
        {
            LayoutRoot.Children.Remove(ui);
        }

        public virtual bool IsFullScreen
        {
            get { return Application.Current.Host.Content.IsFullScreen; }
            set { Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen; }
        }

        #endregion

        #region INavigationLinks Members

        public void MapUri(Uri from, Uri to)
        {
            UriMapper mapper = this.ContentFrame.UriMapper as UriMapper;
            mapper.UriMappings.Insert(0, new UriMapping() { Uri = from, MappedUri = to });
        }

        private HyperlinkButton currentHyperlink = null;

        public void Add(NavigationInfo navigation)
        {
            var hb = GetHyperlinkButtonFor(navigation);
            if (hb == null)
            {
                hb = navigation.AsHyperlinkButton();
                //first check if there is a mapped Uri for this navigation uri
                var uri = (this.ContentFrame.UriMapper as UriMapper).UriMappings.FirstOrDefault(x => x.Uri.OriginalString.Equals(navigation.NavigationUri));
                string pattern = navigation.NavigationUri;
                if (uri != null)
                    pattern = uri.MappedUri.ToString();
                NavigationAuthRule authRule = new NavigationAuthRule()
                {
                    UriPattern = string.Concat("^", pattern, "$")
                };                
                if(navigation.Deny.Count > 0)
                    authRule.Parts.Add(new Deny() { Users = navigation.GetDeniedRolesAsString() });
                if(navigation.Allow.Count > 0)
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

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            IoC.Resolve<IEventAggregator>().GetEvent<SettingsEvent>().Publish(null);
        }

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            IoC.Resolve<IEventAggregator>().GetEvent<HelpEvent>().Publish(null);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PublishSearchEvent();
        }

        private void PublishSearchEvent()
        {
            if (!Watermark.GetIsWatermarked(searchBox))
            {
                IoC.Resolve<IEventAggregator>().GetEvent<SearchTextChangedEvent>().Publish(searchBox.Text);
            }
        }
    }
}