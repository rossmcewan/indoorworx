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

namespace IndoorWorx.Designer.Views
{
    public partial class DesignerView : UserControl, IDesignerView
    {
        public DesignerView(IDesignerPresentationModel model)
        {
            InitializeComponent();
            this.DataContext = model;
            model.View = this;
            model.VideoSelected += new EventHandler<DataEventArgs<Video>>(model_VideoSelected);
        }

        void model_VideoSelected(object sender, DataEventArgs<Video> e)
        {
            var video = e.Value;            
            if (video.IsTelemetryLoaded)
            {
                telemetryChart.LoadTelemetry(video.Telemetry);
            }
            else
            {
                video.TelemetryLoaded += (_sender, _e) =>
                {
                    SmartDispatcher.BeginInvoke(() =>
                    {
                        telemetryChart.LoadTelemetry(video.Telemetry);
                    });
                };
            }
        }

        public IDesignerPresentationModel Model
        {
            get { return this.DataContext as IDesignerPresentationModel; }
        }

        public void AddDesigner()
        {
            DocumentPaneGroup.AddItem(new RadDocumentPane() { Title = Designer.Resources.DesignerResources.NewDesignTitle }, DockPosition.Center);
        }
    }
}
