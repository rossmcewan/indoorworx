﻿using System;
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
using IndoorWorx.Catalog.Views;
using IndoorWorx.Infrastructure.DragDrop;
using IndoorWorx.Library.DragDrop;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Catalog.Resources;

namespace IndoorWorx.Catalog.Pages
{
    public partial class VideosPage : Page
    {
        private IDropTargetHost host;
        private IDropTarget dropTarget;
        bool reloadRequired = true;
        public VideosPage()
        {
            host = IoC.Resolve<IDropTargetHost>();
            dropTarget = new DropTarget(
                (target, payload) =>
                {
                    target.IsBusy = true;
                    ApplicationUser.CurrentUser.AddVideoToLibrary(payload as Video, () => target.IsBusy = false);
                },
                (target, payload) => payload is Video, (target) => ApplicationContext.Current.VideoCount)
            {
                Id = Guid.NewGuid(),
                Image = new Uri("/IndoorWorx.Catalog.Silverlight;component/Images/library.png", UriKind.Relative),
                Title = CatalogResources.MyLibraryOfVideos
            };
            ApplicationContext.Current.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "VideoCount")
                    dropTarget.ItemCount = ApplicationContext.Current.VideoCount;
            };
            InitializeComponent();
            var contentElement = IoC.Resolve<IVideosView>() as UserControl;
            if (contentElement.Parent != null)
            {
                (contentElement.Parent as VideosPage).Content = null;
                reloadRequired = false;
            }
            this.Content = contentElement;
        }

        private IVideosView View
        {
            get { return this.Content as IVideosView; }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (reloadRequired)
                View.Model.Refresh();
            host.AddDropTarget(dropTarget);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            host.RemoveDropTarget(dropTarget);
        }
    }
}
