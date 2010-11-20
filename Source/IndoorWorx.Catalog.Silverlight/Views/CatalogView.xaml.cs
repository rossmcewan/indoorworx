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
using Microsoft.Web.Media.SmoothStreaming;
using Telerik.Windows.Controls;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure;
using Telerik.Windows.Controls.Charting;

namespace IndoorWorx.Catalog.Views
{
    public partial class CatalogView : UserControl, ICatalogView
    {
        public CatalogView(ICatalogPresentationModel model)
        {
            InitializeComponent();
            this.DataContext = model;
            model.View = this;
        }

        #region ICatalogView Members

        public ICatalogPresentationModel Model
        {
            get { return this.DataContext as ICatalogPresentationModel; }
        }

        #endregion

        private TChildItem FindVisualChild<TChildItem>(DependencyObject obj) where TChildItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TChildItem)
                    return (TChildItem)child;
                else
                {
                    TChildItem childOfChild = FindVisualChild<TChildItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private void radTileView_TileStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        private void selectedVideoTile_TileStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e) 
        {
            var item = sender as RadTileViewItem;
            if (item != null)
            {
                var control = item.Content as RadFluidContentControl;
                if (control != null)
                {
                    switch (item.TileState)
                    {
                        case TileViewItemState.Maximized:
                            control.State = FluidContentControlState.Large;
                            break;
                        case TileViewItemState.Minimized:
                            control.State = FluidContentControlState.Small;
                            break;
                        case TileViewItemState.Restored:
                            control.State = FluidContentControlState.Normal;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void profileChart_Loaded(object sender, RoutedEventArgs e)
        {
            var chart = sender as RadChart;
            var video = chart.DataContext as Video;

            chart.DefaultView.ChartArea.EnableAnimations = false;
            AreaSeriesDefinition lineSeries = new AreaSeriesDefinition();
            lineSeries.ShowItemLabels = false;
            lineSeries.ShowPointMarks = false;

            SeriesMapping dataMapping = new SeriesMapping();
            dataMapping.SeriesDefinition = lineSeries;
            dataMapping.ItemMappings.Add(new ItemMapping("TimePositionAsDateTime", DataPointMember.XValue));
            dataMapping.ItemMappings.Add(new ItemMapping("PercentageThreshold", DataPointMember.YValue));
            dataMapping.ItemMappings[1].SamplingFunction = ChartSamplingFunction.KeepExtremes;

            chart.SeriesMappings.Add(dataMapping);

            chart.DefaultView.ChartArea.Foreground = new SolidColorBrush(Colors.Green);
            chart.DefaultView.ChartArea.ZoomScrollSettingsX.ScrollMode = ScrollMode.None;
            chart.DefaultView.ChartArea.ZoomScrollSettingsY.ScrollMode = ScrollMode.None;

            chart.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisX.DefaultLabelFormat = "#VAL{HH:mm}";
            chart.DefaultView.ChartArea.AxisY.DefaultLabelFormat = "#VAL{p0}";

            chart.DefaultView.ChartArea.AxisY.AutoRange = false;
            chart.DefaultView.ChartArea.AxisY.MajorGridLinesVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisY.MinorGridLinesVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisY.StripLinesVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = Visibility.Collapsed;

            chart.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;
            chart.SamplingSettings.SamplingThreshold = 1000;
            chart.DefaultView.ChartArea.EnableAnimations = false;

            if (video.SelectedTrainingSet.IsTelemetryLoaded)
            {
                var max = video.SelectedTrainingSet.Telemetry.Max(x => x.PercentageThreshold);
                chart.DefaultView.ChartArea.AxisY.AddRange(0, max, 0.2);
                chart.DefaultView.ChartArea.AxisX.MinValue = DateTime.Today.ToOADate();
                chart.DefaultView.ChartArea.AxisX.MaxValue = video.SelectedTrainingSet.Telemetry.Max(x => x.TimePositionAsDateTime).ToOADate();
                chart.ItemsSource = video.SelectedTrainingSet.Telemetry;
            }
            else
            {
                video.SelectedTrainingSet.TelemetryLoaded += (_sender, _e) =>
                    {
                        SmartDispatcher.BeginInvoke(() =>
                            {
                                var max = video.SelectedTrainingSet.Telemetry.Max(x => x.PercentageThreshold);
                                chart.DefaultView.ChartArea.AxisY.AddRange(0, max, 0.2);
                                chart.DefaultView.ChartArea.AxisX.MinValue = DateTime.Today.ToOADate();
                                chart.DefaultView.ChartArea.AxisX.MaxValue = video.SelectedTrainingSet.Telemetry.Max(x => x.TimePositionAsDateTime).ToOADate();
                                chart.ItemsSource = video.SelectedTrainingSet.Telemetry;
                            });
                    };
            }
        }
    }
}
