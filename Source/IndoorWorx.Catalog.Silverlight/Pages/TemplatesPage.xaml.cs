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
using System.Windows.Navigation;
using IndoorWorx.Infrastructure.DragDrop;
using IndoorWorx.Infrastructure;
using IndoorWorx.Catalog.Resources;
using IndoorWorx.Library.DragDrop;

namespace IndoorWorx.Catalog.Pages
{
    public partial class TemplatesPage : Page
    {
        private IDropTargetHost host;
        private IDropTarget dropTarget;
        public TemplatesPage()
        {
            host = IoC.Resolve<IDropTargetHost>();
            dropTarget = new DropTarget(
                (target, payload) =>
                {
                    MessageBox.Show("This should not be called");
                },
                (target, payload) => false, (target) => 0)
            {
                Id = Guid.NewGuid(),
                Image = new Uri("/IndoorWorx.Catalog.Silverlight;component/Images/templates.png", UriKind.Relative),
                Title = CatalogResources.MyLibraryOfTemplates
            };
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            host.AddDropTarget(dropTarget);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            host.RemoveDropTarget(dropTarget);
        }
    }
}
