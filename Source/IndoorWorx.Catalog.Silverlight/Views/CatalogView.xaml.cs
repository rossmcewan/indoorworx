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
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure;
using Telerik.Windows.Controls.Charting;
using IndoorWorx.Library.Controls;

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

        private void RadDocking_PreviewClose(object sender, Telerik.Windows.Controls.Docking.StateChangeEventArgs e)
        {            
        }

        private void RadDocking_Close(object sender, Telerik.Windows.Controls.Docking.StateChangeEventArgs e)
        {

        }

        private void trainingSetsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //Model.OnTrainingSetSelectionChanged();
        }

        private void radTileView_TileStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //var tileViewItem = e.Source as RadTileViewItem;
            //if (tileViewItem != null && tileViewItem.TileState == TileViewItemState.Maximized)
            //{
            //    Model.OnVideoSelectionChanged();
            //}
        }

        private void CatalogTreeControl_SelectionChanged(object sender, EventArgs e)
        {
            //Model.OnVideoSelectionChanged();
        }                
    }
}
