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

        private void selectedVideoTile_TileStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e) 
        {
            var item = sender as RadTileViewItem;
            if (item != null)
            {
                var control = item.Content as RadFluidContentControl;
                if (control != null)
                {
                    switch (item.TileState)
                    {
                        case TileViewItemState.Maximized:
                            control.State = FluidContentControlState.Large;
                            break;
                        case TileViewItemState.Minimized:
                            control.State = FluidContentControlState.Small;
                            break;
                        case TileViewItemState.Restored:
                            control.State = FluidContentControlState.Normal;
                            break;
                        default:
                            break;
                    }
                }
            }
        }        
    }
}
