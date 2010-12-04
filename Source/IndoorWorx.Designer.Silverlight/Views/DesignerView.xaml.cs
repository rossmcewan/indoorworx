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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Docking;
using IndoorWorx.Infrastructure;
using Telerik.Windows.Controls.Charting;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Web.Media.SmoothStreaming;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Designer.Domain;
using IndoorWorx.Designer.Controls;

namespace IndoorWorx.Designer.Views
{
    public partial class DesignerView : UserControl, IDesignerView
    {
        public DesignerView(IDesignerPresentationModel model)
        {
            InitializeComponent();
            this.DataContext = model;
            model.View = this;
            model.EntriesChanged += new EventHandler(model_EntriesChanged);
        }

        void model_EntriesChanged(object sender, EventArgs e)
        {
            designedTelemetryChart.LoadTelemetry(Model.GetDesignedTelemetry());
        }        

        public IDesignerPresentationModel Model
        {
            get { return this.DataContext as IDesignerPresentationModel; }
        }

        public void AddDesigner(TrainingSet forVideo)
        {
            var documentPane = new RadDocumentPane()
            {
                Title = Designer.Resources.DesignerResources.NewDesignTitle,
                Content = new TrainingSetDesignControl() { Model = new TrainingSetDesign() { FromTrainingSet = forVideo } }
            };
            
            DocumentPaneGroup.AddItem(documentPane, DockPosition.Center);                
        }

        private void RadDocking_PreviewClose(object sender, StateChangeEventArgs e)
        {

        }

        private void RadDocking_Close(object sender, StateChangeEventArgs e)
        {

        }
    }
}
