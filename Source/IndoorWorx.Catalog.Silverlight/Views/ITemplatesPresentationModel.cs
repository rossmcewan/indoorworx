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

namespace IndoorWorx.Catalog.Views
{
    public interface ITemplatesPresentationModel
    {
        ITemplatesView View { set; get; }

        IDropTarget TemplateDropTarget { get; set; }

        void Refresh();

        void OnMyLibrarySelected();
    }
}
