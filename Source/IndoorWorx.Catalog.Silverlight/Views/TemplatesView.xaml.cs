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
using IndoorWorx.Catalog.Helpers;

namespace IndoorWorx.Catalog.Views
{
    public partial class TemplatesView : UserControl, ITemplatesView
    {
        public TemplatesView(ITemplatesPresentationModel model)
        {
            this.DataContext = model;            
            InitializeComponent();
            InitializeTemplateDropTargetList(this.templateDropTargetList);
            model.View = this;
        }

        public ITemplatesPresentationModel Model
        {
            get { return this.DataContext as ITemplatesPresentationModel; }
        }

        private void InitializeTemplateDropTargetList(ListBox listBox)
        {
            RadDragAndDropManager.AddDropQueryHandler(listBox, DropTargetHelper.OnDropQuery);
            RadDragAndDropManager.AddDropInfoHandler(listBox, DropTargetHelper.OnDropInfo);
        }
    }
}
