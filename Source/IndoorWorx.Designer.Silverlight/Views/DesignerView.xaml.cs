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
using Microsoft.Practices.ServiceLocation;

namespace IndoorWorx.Designer.Views
{
    public partial class DesignerView : UserControl, IDesignerView
    {
        public event EventHandler EntriesChangedOnView;

        private readonly IServiceLocator serviceLocator;
        public DesignerView(IServiceLocator serviceLocator, IDesignerPresentationModel model)
        {
            this.serviceLocator = serviceLocator;
            InitializeComponent();
            this.DataContext = model;
            model.View = this;
            model.EntriesChanged += new EventHandler(model_EntriesChanged);
        }

        Color[] colors = new Color[] { Colors.Gray, Colors.LightGray };
        Random random = new Random();
        void model_EntriesChanged(object sender, EventArgs e)
        {
            RefreshChart();
        }

        private void RefreshChart()
        {
            //designedTelemetryChart.LoadTelemetry(Model.GetDesignedTelemetry());
            //designedTelemetryChart.DefaultView.ChartArea.Annotations.Clear();
            //double seconds = 0;
            //int counter = 0;
            //foreach (var entry in Model.Entries)
            //{
            //    var fromDate = DateTimeHelper.ZeroTime.Add(TimeSpan.FromSeconds(seconds));
            //    var from = fromDate.ToOADate();
            //    var length = entry.TimeEnd - entry.TimeStart;
            //    var to = fromDate.Add(length).ToOADate();
            //    seconds += length.TotalSeconds;

            //    var zone = new MarkedZone(from, to, 0, designedTelemetryChart.DefaultView.ChartArea.AxisY.MaxValue);
            //    zone.Tag = entry;
            //    zone.Background = new SolidColorBrush(colors[Math.Min(1, counter++ % 2)]);
            //    designedTelemetryChart.DefaultView.ChartArea.Annotations.Add(zone);                
            //}
            //designedTelemetryChart.DefaultView.ChartArea.ItemClick += new EventHandler<ChartItemClickEventArgs>(ChartArea_ItemClick);
            //if (EntriesChangedOnView != null)
            //    EntriesChangedOnView(this, EventArgs.Empty);
        }

        MarkedZone lastSelected;
        Brush lastBackground;
        void ChartArea_ItemClick(object sender, ChartItemClickEventArgs e)
        {
            //if (lastSelected != null)
            //    lastSelected.Background = lastBackground;
            //lastSelected = designedTelemetryChart.DefaultView.ChartArea.Annotations.OfType<MarkedZone>().FirstOrDefault(
            //    x => x.StartX <= e.DataPoint.XValue && x.EndX >= e.DataPoint.XValue);
            //lastBackground = lastSelected.Background;
            //lastSelected.Background = new SolidColorBrush(Colors.Green);
            //Model.SelectedEntry = lastSelected.Tag as TrainingSetDesignEntry;            
        }

        public IDesignerPresentationModel Model
        {
            get { return this.DataContext as IDesignerPresentationModel; }
        }

        public void AddDesigner(Video forVideo)
        {
            //var existingPane = DocumentPaneGroup.Items.OfType<RadDocumentPane>().FirstOrDefault(x => x.Tag == forVideo);
            //if (existingPane == null)
            //{
            //    var view = serviceLocator.GetInstance<IDesignerSelectorView>();
            //    forVideo.SelectedTrainingSet = forVideo.TrainingSets.FirstOrDefault();
            //    view.Model.Source = forVideo;
                
            //    var documentPane = new RadDocumentPane()
            //    {
            //        Tag = forVideo,
            //        Title = forVideo.Title,
            //        Content = view                    
            //    };
            //    DocumentPaneGroup.Items.Clear();
            //    DocumentPaneGroup.AddItem(documentPane, DockPosition.Center);
            //}
            //else
            //{
            //    existingPane.IsSelected = true;
            //}
        }

        private void RadDocking_PreviewClose(object sender, StateChangeEventArgs e)
        {

        }

        private void RadDocking_Close(object sender, StateChangeEventArgs e)
        {

        }

        private void RadSlider_DragCompleted(object sender, RadDragCompletedEventArgs e)
        {
            RefreshChart();
        }
    }
}
