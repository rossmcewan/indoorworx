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

namespace IndoorWorx.Library.Controls
{
    public class TelemetryChart : RadChart
    {
        public TelemetryChart()
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

            CustomGridLine line = new CustomGridLine();
            line.XIntercept = 0;
            line.Visibility = System.Windows.Visibility.Visible;
            line.Stroke = new SolidColorBrush(Colors.Green);
            line.StrokeThickness = 2;
            this.DefaultView.ChartArea.AxisY.AutoRange = false;
            this.DefaultView.ChartArea.Annotations.Add(line);            
        }
    }
}
