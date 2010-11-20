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
using DynamicNavigation;

namespace IndoorWorx.Player.Views.Dynamic
{
    public partial class PlayerPage : DynamicPage
    {
        bool reloadRequired = true;
        public PlayerPage()
        {
            InitializeComponent();
            var contentElement = IoC.Resolve<IPlayerView>() as UserControl;
            if (contentElement.Parent != null)
            {
                (contentElement.Parent as PlayerPage).Content = null;
                reloadRequired = false;
            }
            this.Content = contentElement;
        }

        private IPlayerView View
        {
            get { return this.Content as IPlayerView; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

    }
}
