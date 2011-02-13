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
using IndoorWorx.Infrastructure;

namespace IndoorWorx.Designer.Views
{
    public partial class DesignerSelectorView : UserControl, IDesignerSelectorView
    {
        public DesignerSelectorView(IDesignerSelectorPresentationModel presentationModel)
        {
            this.Loaded += new RoutedEventHandler(DesignerSelectorView_Loaded);
            InitializeComponent();
            this.DataContext = presentationModel;
        }

        void DesignerSelectorView_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public IDesignerSelectorPresentationModel Model
        {
            get { return this.DataContext as IDesignerSelectorPresentationModel; }
        }

        private void TelemetryChart_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTelemetryOnChart();
        }

        private void LoadTelemetryOnChart()
        {
            //if (Model.Source.SelectedTrainingSet != null)
            //{
            //    if (Model.Source.SelectedTrainingSet.IsTelemetryLoaded)
            //        telemetryChart.LoadTelemetry(Model.Source.SelectedTrainingSet.Telemetry);
            //    else
            //    {
            //        Model.Source.SelectedTrainingSet.TelemetryLoaded += (_sender, _e) =>
            //        {
            //            SmartDispatcher.BeginInvoke(() => telemetryChart.LoadTelemetry(Model.Source.SelectedTrainingSet.Telemetry));
            //        };
            //        Model.Source.SelectedTrainingSet.LoadTelemetry();
            //    }
            //}
        }

        private void RadComboBox_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadTelemetryOnChart();
            Model.OnTrainingSetSelectionChanged();
        }

        private void slider_SelectionStartChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                fromMediaElement.Position = TimeSpan.FromSeconds(this.Model.SelectionStart.GetValueOrDefault());
            }
            catch { }
        }

        private void slider_SelectionEndChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                toMediaElement.Position = TimeSpan.FromSeconds(this.Model.SelectionEnd.GetValueOrDefault());
            }
            catch { }
        }
    }
}
