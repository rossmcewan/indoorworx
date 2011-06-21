using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls.DragDrop;
using IndoorWorx.Infrastructure.DragDrop;

namespace IndoorWorx.Catalog.Helpers
{
    public class DropTargetHelper
    {
        public static void OnDropQuery(object sender, DragDropQueryEventArgs e)
        {
            var destination = e.Options.Destination as ListBox;
            if (e.Options.Status == DragStatus.DropDestinationQuery &&
                destination != null)
            {
                var dropTarget = destination.DataContext as IDropTarget;
                e.QueryResult = dropTarget.CanDrop(e.Options.Payload) && !dropTarget.IsBusy;
                e.Handled = true;
            }
        }

        public static void OnDropInfo(object sender, DragDropEventArgs e)
        {
            var destination = e.Options.Destination as ListBox;
            if (e.Options.Status == DragStatus.DropComplete &&
                 destination != null)
            {
                var dropTarget = destination.DataContext as IDropTarget;
                dropTarget.OnDropped(e.Options.Payload);
            }
        }       
    }
}
