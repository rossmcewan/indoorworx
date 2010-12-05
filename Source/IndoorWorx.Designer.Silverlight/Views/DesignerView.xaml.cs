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
using IndoorWorx.Designer.Models;
using IndoorWorx.Designer.Controls;
using IndoorWorx.Infrastructure.Helpers;

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

        Random random = new Random();
        void model_EntriesChanged(object sender, EventArgs e)
        {
            designedTelemetryChart.LoadTelemetry(Model.GetDesignedTelemetry());
            designedTelemetryChart.DefaultView.ChartArea.Annotations.Clear();
            double seconds = 0;
            foreach (var entry in Model.Entries)
            {
                var fromDate = DateTimeHelper.ZeroTime.Add(TimeSpan.FromSeconds(seconds));
                var from = fromDate.ToOADate();
                var length = entry.TimeEnd - entry.TimeStart;
                var to = fromDate.Add(length).ToOADate();
                seconds += length.TotalSeconds;

                //var from = DateTimeHelper.ZeroTime.Add(entry.TimeStart).Add(TimeSpan.FromSeconds(seconds)).ToOADate();
                //var to = DateTimeHelper.ZeroTime.Add(entry.TimeEnd).Add(TimeSpan.FromSeconds(seconds)).ToOADate();
                var zone = new MarkedZone(from, to, 0, designedTelemetryChart.DefaultView.ChartArea.AxisY.MaxValue);
                Color newColor = Color.FromArgb(255, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                zone.Background = new SolidColorBrush(newColor);
                designedTelemetryChart.DefaultView.ChartArea.Annotations.Add(zone);

                zone.MouseRightButtonDown += new MouseButtonEventHandler(zone_MouseRightButtonDown);
                zone.MouseRightButtonUp += new MouseButtonEventHandler(zone_MouseRightButtonUp);
                RadContextMenu.SetContextMenu(zone, new RadContextMenu() { ItemsSource = new List<RadMenuItem>() { new RadMenuItem() { Header = "Test" } } });
            }
        }

        void zone_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        void zone_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }                

        public IDesignerPresentationModel Model
        {
            get { return this.DataContext as IDesignerPresentationModel; }
        }

        public void AddDesigner(Video forVideo)
        {
            var existingPane = DocumentPaneGroup.Items.OfType<RadDocumentPane>().FirstOrDefault(x => x.Tag == forVideo);
            if (existingPane == null)
            {
                var documentPane = new RadDocumentPane()
                {
                    Tag = forVideo,
                    Title = forVideo.Title,
                    Content = new TrainingSetDesignControl() { Model = new TrainingSetDesign() { Source = forVideo } }
                };

                DocumentPaneGroup.AddItem(documentPane, DockPosition.Center);
            }
            else
            {
                existingPane.IsSelected = true;
            }
        }

        private void RadDocking_PreviewClose(object sender, StateChangeEventArgs e)
        {

        }

        private void RadDocking_Close(object sender, StateChangeEventArgs e)
        {

        }
    }
}
