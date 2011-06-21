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

namespace IndoorWorx.Infrastructure.DragDrop
{
    public interface IDropTarget
    {
        Guid Id { get; set; }

        Uri Image { get; set; }

        string Title { get; set; }

        bool CanDrop(object payload);

        void OnDropped(object payload);

        int ItemCount { get; set; }
    }
}
