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

    /// <summary>
    /// <see cref="UserControl"/> class providing the main UI for the application.
    /// </summary>
    public partial class Shell : UserControl, IShell, INavigationLinks, IDropTargetHost
    {
        /// <summary>
        /// Creates a new <see cref="MainPage"/> instance.
        /// </summary>
        public Shell(IUnityContainer unityContainer)
        {            
            InitializeComponent();

            //Telerik.Windows.Controls.DragDrop.RadDragAndDropManager.AddDropQueryHandler(dropHere, dropHere_DropQuery);
            //Telerik.Windows.Controls.DragDrop.RadDragAndDropManager.AddDropInfoHandler(dropHere, dropHere_DropInfo);

            WebContext.Current.Authentication.LoggedIn += new EventHandler<AuthenticationEventArgs>(Authentication_LoggedIn);
            WebContext.Current.Authentication.LoggedOut += new EventHandler<AuthenticationEventArgs>(Authentication_LoggedOut);
            this.loginContainer.Child = new LoginStatus();
            unityContainer.RegisterInstance<INavigationLinks>(this);
            unityContainer.RegisterInstance<IDropTargetHost>(this);
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
            var busyIndicator = new BusyIndicator();
            busyIndicator.Content = this;
            busyIndicator.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            busyIndicator.VerticalContentAlignment = VerticalAlignment.Stretch;
            
            Application.Current.RootVisual = busyIndicator;
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

        private void listBox_DropQuery(object sender, Telerik.Windows.Controls.DragDrop.DragDropQueryEventArgs e)
        {
            var destination = e.Options.Destination as ListBox;
            if (e.Options.Status == DragStatus.DropDestinationQuery &&
                destination != null)
            {
                OnEntering(destination);
                var dropTarget = destination.DataContext as IDropTarget;
                e.QueryResult = dropTarget.CanDrop(e.Options.Payload);
                e.Handled = true;
            }            
        }

        private void listBox_DropInfo(object sender, Telerik.Windows.Controls.DragDrop.DragDropEventArgs e)
        {
            // if we are dropping on the appropriate listbox, then add the dragged item to it.
            var destination = e.Options.Destination as ListBox;
            if (e.Options.Status == Telerik.Windows.Controls.DragDrop.DragStatus.DropComplete &&
                 destination != null)
            {
                var dropTarget = destination.DataContext as IDropTarget;
                dropTarget.OnDropped(e.Options.Payload);
            }
        }

        public void AddDropTarget(IDropTarget dropTarget)
        {
            var listBox = new ListBox();
            listBox.Style = Application.Current.Resources[ResourceDictionaryKeys.DropTargetStyle] as Style;
            listBox.DataContext = dropTarget;
            listBox.MouseEnter += (sender, e) =>
                {
                    OnEntering(listBox);                    
                };
            listBox.MouseLeave += (sender, e) =>
                {
                    OnLeaving(listBox);                    
                };
            RadDragAndDropManager.AddDropQueryHandler(listBox, listBox_DropQuery);
            RadDragAndDropManager.AddDropInfoHandler(listBox, listBox_DropInfo);
            DropTargetsStackPanel.Children.Add(listBox);
        }

        private void OnLeaving(ListBox listBox)
        {
            DropTargetsToolTip.Text = string.Empty;
            listBox.Opacity = 0.5;
        }

        private void OnEntering(ListBox listBox)
        {
            foreach (var child in DropTargetsStackPanel.Children)
            {
                OnLeaving(child as ListBox);
            }
            var dropTarget = listBox.DataContext as IDropTarget;
            DropTargetsToolTip.Text = dropTarget.Title;
            listBox.Opacity = 1;
        }

        public void RemoveDropTarget(IDropTarget dropTarget)
        {
            throw new NotImplementedException();
        }
    }
}