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
using Telerik.Windows.Controls.DragDrop;

namespace IndoorWorx.Catalog.Views
{
    public partial class VideoCatalogView : UserControl, IVideoCatalogView
    {
        public VideoCatalogView(IVideoCatalogPresentationModel model)
        {
            this.DataContext = model;
            InitializeComponent();
            model.View = this;
        }

        public IVideoCatalogPresentationModel Model
        {
            get { return this.DataContext as IVideoCatalogPresentationModel; }
        }

        private void Video_DragQuery(object sender, Telerik.Windows.Controls.DragDrop.DragDropQueryEventArgs e)
        {
            if (e.Options.Status == DragStatus.DragQuery)
            {
                var draggedItem = e.Options.Source;
                ContentControl dragCue = new ContentControl();
                dragCue.Content = draggedItem.DataContext;
                dragCue.ContentTemplate = this.Resources["DragCueTemplate"] as DataTemplate;
                e.Options.DragCue = dragCue;
            }
        }

        private void ItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            RadDragAndDropManager.RemoveDragQueryHandler(sender as DependencyObject, Video_DragQuery);
            RadDragAndDropManager.AddDragQueryHandler(sender as DependencyObject, Video_DragQuery);
        }
    }
}
