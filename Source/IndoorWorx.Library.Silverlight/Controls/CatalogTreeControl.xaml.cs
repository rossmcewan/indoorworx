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
using System.Collections;
using IndoorWorx.Infrastructure.Navigation;
using Telerik.Windows.Controls;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Library.Controls
{
    public partial class CatalogTreeControl : UserControl
    {
        public CatalogTreeControl()
        {
            InitializeComponent();
        }

        private ICategoryTreeControlModel Model
        {
            get { return this.DataContext as ICategoryTreeControlModel; }
        }

        public IEnumerable<IMenuItem> ContextMenuItemsSource
        {
            get { return (IEnumerable<IMenuItem>)GetValue(ContextMenuItemsSourceProperty); }
            set { SetValue(ContextMenuItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContextMenuItemsSourceProperty =
            DependencyProperty.Register("ContextMenuItemsSource", typeof(IEnumerable<IMenuItem>), typeof(CatalogTreeControl), new PropertyMetadata(ContextMenuItemsSourceChanged));

        private static void ContextMenuItemsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var treeControl = sender as CatalogTreeControl;
            treeControl.catalogTreeContextMenu.ItemsSource = args.NewValue as IEnumerable<IMenuItem>;
        }

        private Video ClickedVideo
        {
            get
            {
                var treeItem = catalogTreeContextMenu.GetClickedElement<RadTreeViewItem>();

                if (treeItem == null)
                    return null;

                var selectedTrainingSet = treeItem.DataContext as Video;

                return selectedTrainingSet;
            }
        }

        private void catalogTreeContextMenu_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var radMenuItem = e.Source as RadMenuItem;
            var menuItem = radMenuItem.DataContext as IMenuItem;
            if (menuItem.Command != null)
                menuItem.Command.Execute(ClickedVideo);
        }

        private void catalogTreeContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (ClickedVideo == null)
                catalogTreeContextMenu.IsEnabled = false;
            else
            {
                catalogTreeContextMenu.IsEnabled = true;
            }
        }
    }
}
