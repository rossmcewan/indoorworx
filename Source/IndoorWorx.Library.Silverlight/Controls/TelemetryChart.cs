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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Charting;
using IndoorWorx.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace IndoorWorx.Library.Controls
{
    public class TelemetryChart : RadChart
    {
        private bool isChartInitialized = false;
        private bool isMarkerLoaded = false;

        private void InitialiseChart()
        {
            AreaSeriesDefinition lineSeries = new AreaSeriesDefinition();
            lineSeries.ShowItemLabels = false;
            lineSeries.ShowPointMarks = false;

            SeriesMapping dataMapping = new SeriesMapping();
            dataMapping.SeriesDefinition = lineSeries;
            dataMapping.ItemMappings.Add(new ItemMapping("XValue", DataPointMember.XValue));
            dataMapping.ItemMappings.Add(new ItemMapping("YValue", DataPointMember.YValue));
            dataMapping.ItemMappings[1].SamplingFunction = ChartSamplingFunction.KeepExtremes;

            this.SeriesMappings.Add(dataMapping);

            this.DefaultView.ChartArea.ZoomScrollSettingsX.ScrollMode = ScrollMode.None;
            this.DefaultView.ChartArea.ZoomScrollSettingsY.ScrollMode = ScrollMode.None;

            this.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Collapsed;
            this.DefaultView.ChartArea.AxisX.DefaultLabelFormat = "#VAL{HH:mm:ss}";
            this.DefaultView.ChartArea.AxisY.DefaultLabelFormat = "#VAL{p0}";

            this.AxisElementBrush = new SolidColorBrush(Colors.White);
            this.AxisForeground = new SolidColorBrush(Colors.White);
            this.DefaultView.ChartArea.AxisY.MajorGridLinesVisibility = Visibility.Collapsed;
            this.DefaultView.ChartArea.AxisY.MinorGridLinesVisibility = Visibility.Collapsed;
            this.DefaultView.ChartArea.AxisY.StripLinesVisibility = Visibility.Collapsed;
            this.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = Visibility.Collapsed;

            this.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;
            this.SamplingSettings.SamplingThreshold = 1000;
            this.DefaultView.ChartArea.EnableAnimations = false;
        }


        private void InitialiseMarker()
        {
            CustomGridLine line = new CustomGridLine();
            line.XIntercept = 0;
            line.Visibility = System.Windows.Visibility.Visible;
            line.Stroke = new SolidColorBrush(Colors.Green);
            line.StrokeThickness = 2;
            this.DefaultView.ChartArea.AxisY.AutoRange = false;
            this.DefaultView.ChartArea.Annotations.Add(line);    
        }

        public void LoadChart(ICollection<Telemetry> telemetry, bool hasMarker)
        {
            if (!isChartInitialized)
                InitialiseChart();
            
            if (hasMarker && ! isMarkerLoaded)
                InitialiseMarker();

            LoadTelemetry(telemetry);
        }

        public void LoadTelemetry(ICollection<Telemetry> telemetry)
        {
            var max = telemetry.Max(x => x.PercentageThreshold);
            this.DefaultView.ChartArea.AxisY.AddRange(0, max, 0.2);
            this.DefaultView.ChartArea.AxisX.MinValue = DateTime.Today.ToOADate();
            this.DefaultView.ChartArea.AxisX.MaxValue = telemetry.Max(x => x.TimePositionAsDateTime).ToOADate();
            this.ItemsSource = telemetry;
        }


    }
}
