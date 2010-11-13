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
using Microsoft.Web.Media.SmoothStreaming;
using Telerik.Windows.Controls;

namespace IndoorWorx.Catalog.Views
{
    public partial class CatalogView : UserControl, ICatalogView
    {
        public CatalogView(ICatalogPresentationModel model)
        {
            InitializeComponent();            
            this.DataContext = model;
            model.View = this;
        }

        #region ICatalogView Members

        public ICatalogPresentationModel Model
        {
            get { return this.DataContext as ICatalogPresentationModel; }
        }

        #endregion

        private void SmoothStreamingMediaElement_SmoothStreamingErrorOccurred(object sender, Microsoft.Web.Media.SmoothStreaming.SmoothStreamingErrorEventArgs e)
        {

        }

        private void SmoothStreamingMediaElement_ClipError(object sender, Microsoft.Web.Media.SmoothStreaming.ClipEventArgs e)
        {

        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var player = GetPlayer();
            Model.PlaySelectedPreview(() => player.Play());
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {            
            var player = GetPlayer();
            Model.StopSelectedPreview(() => player.Stop());
        }

        private SmoothStreamingMediaElement GetPlayer()
        {
            var item = radTileView.ItemContainerGenerator.ContainerFromItem(Model.SelectedCategory.SelectedCatalog.SelectedVideo ?? Model.SelectedCategory.SelectedCatalog.Videos.FirstOrDefault());

            var player = FindVisualChild<SmoothStreamingMediaElement>(item);

            return player;
        }

        private TChildItem FindVisualChild<TChildItem>(DependencyObject obj) where TChildItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TChildItem)
                    return (TChildItem)child;
                else
                {
                    TChildItem childOfChild = FindVisualChild<TChildItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private void radTileView_TileStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }
    }
}
