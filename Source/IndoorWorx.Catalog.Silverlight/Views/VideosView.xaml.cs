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
using IndoorWorx.Infrastructure.DragDrop;
using IndoorWorx.Catalog.Helpers;

namespace IndoorWorx.Catalog.Views
{
    public partial class VideosView : UserControl, IVideosView
    {
        public VideosView(IVideosPresentationModel model)
        {
            this.DataContext = model;
            InitializeComponent();
            model.View = this;
            InitializeVideoDropTargetList(this.videoDropTargetList);
        }

        private void InitializeVideoDropTargetList(ListBox listBox)
        {            
            RadDragAndDropManager.AddDropQueryHandler(listBox, DropTargetHelper.OnDropQuery);
            RadDragAndDropManager.AddDropInfoHandler(listBox, DropTargetHelper.OnDropInfo);
        }        

        public IVideosPresentationModel Model
        {
            get { return DataContext as IVideosPresentationModel; }
        }

        private void VideosContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            UpdateSelectedLinks(e.Uri);
        }

        private void UpdateSelectedLinks(Uri uri)
        {            
            for(int i=0;i<CategoriesItemsControl.Items.Count;i++)
            {
                var child = VisualTreeHelper.GetChild(this.CategoriesItemsControl.ItemContainerGenerator.ContainerFromIndex(i), 0);
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    var compareTo = uri.ToString();
                    if (hb.NavigateUri.ToString().Equals(compareTo))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }     
    }
}
