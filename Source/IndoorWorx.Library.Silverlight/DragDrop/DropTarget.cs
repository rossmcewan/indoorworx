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
using IndoorWorx.Infrastructure.DragDrop;

namespace IndoorWorx.Library.DragDrop
{
    public class DropTarget : IDropTarget
    {
        private Action<object> onDropped;

        private Func<object, bool> canDrop;

        public DropTarget(Action<object> onDropped, Func<object, bool> canDrop)
        {
            this.onDropped = onDropped;
            this.canDrop = canDrop;
        }

        public Guid Id { get; set; }

        public Uri Image { get; set; }

        public string Title { get; set; }

        public void OnDropped(object payload)
        {
            if (onDropped != null)
                onDropped(payload);
        }

        public bool CanDrop(object payload)
        {
            if (canDrop != null)
                return canDrop(payload);
            return true;
        }
    }
}
