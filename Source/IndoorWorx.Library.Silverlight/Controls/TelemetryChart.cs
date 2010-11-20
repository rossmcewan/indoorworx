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
        private CustomGridLine line;
        public TelemetryChart():base()
        {            
            AreaSeriesDefinition lineSeries = new AreaSeriesDefinition();
            lineSeries.ShowItemLabels = false;
            lineSeries.ShowPointMarks = false;

            SeriesMapping dataMapping = new SeriesMapping();
            dataMapping.SeriesDefinition = lineSeries;
            dataMapping.ItemMappings.Add(new ItemMapping("TimePositionAsDateTime", DataPointMember.XValue));
            dataMapping.ItemMappings.Add(new ItemMapping("PercentageThreshold", DataPointMember.YValue));
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

            line = new CustomGridLine();
            line.XIntercept = 0;
            line.Visibility = System.Windows.Visibility.Visible;
            line.Stroke = new SolidColorBrush(Colors.Green);
            line.StrokeThickness = 2;
            this.DefaultView.ChartArea.AxisY.AutoRange = false;
        }

        public void LoadTelemetry(ICollection<Telemetry> telemetry)
        {
            var max = telemetry.Max(x => x.PercentageThreshold);
            this.DefaultView.ChartArea.AxisY.AddRange(0, max, 0.2);
            this.DefaultView.ChartArea.AxisX.MinValue = DateTime.Today.ToOADate();
            this.DefaultView.ChartArea.AxisX.MaxValue = telemetry.Max(x => x.TimePositionAsDateTime).ToOADate();
            this.ItemsSource = telemetry;
        }

        public Visibility XAxisLabelVisibility
        {
            get { return (Visibility)GetValue(XAxisLabelVisibilityProperty); }
            set { SetValue(XAxisLabelVisibilityProperty, value); }
        }

        public static readonly DependencyProperty XAxisLabelVisibilityProperty =
            DependencyProperty.Register("XAxisLabelVisibility", typeof(Visibility), typeof(TelemetryChart), new PropertyMetadata(Visibility.Visible, (sender, e) =>
                {
                    var chart = sender as TelemetryChart;
                    chart.DefaultView.ChartArea.AxisX.AxisLabelsVisibility = chart.XAxisLabelVisibility;
                }));

        public Visibility YAxisLabelVisibility
        {
            get { return (Visibility)GetValue(YAxisLabelVisibilityProperty); }
            set { SetValue(YAxisLabelVisibilityProperty, value); }
        }

        public static readonly DependencyProperty YAxisLabelVisibilityProperty =
            DependencyProperty.Register("YAxisLabelVisibility", typeof(Visibility), typeof(TelemetryChart), new PropertyMetadata(Visibility.Visible, (sender, e) =>
                {
                    var chart = sender as TelemetryChart;
                    chart.DefaultView.ChartArea.AxisY.AxisLabelsVisibility = chart.YAxisLabelVisibility;
                }));

        public double CurrentProgress
        {
            get { return (double)GetValue(CurrentProgressProperty); }
            set { SetValue(CurrentProgressProperty, value); }
        }

        public static readonly DependencyProperty CurrentProgressProperty =
            DependencyProperty.Register("CurrentProgress", typeof(double), typeof(TelemetryChart), new PropertyMetadata(0.0, CurrentProgressChanged));

        private static void CurrentProgressChanged(DependencyObject src, DependencyPropertyChangedEventArgs args)
        {
            var telemetryChart = src as TelemetryChart;
            telemetryChart.line.XIntercept = telemetryChart.CurrentProgress;
        }

        public bool ShowProgress
        {
            get { return (bool)GetValue(ShowProgressProperty); }
            set { SetValue(ShowProgressProperty, value); }
        }

        public static readonly DependencyProperty ShowProgressProperty =
            DependencyProperty.Register("ShowProgress", typeof(bool), typeof(TelemetryChart), new PropertyMetadata(false, ShowProgressChanged));

        private static void ShowProgressChanged(DependencyObject src, DependencyPropertyChangedEventArgs args)
        {                        
            var telemetryChart = src as TelemetryChart;
            if (telemetryChart.ShowProgress)
                telemetryChart.DefaultView.ChartArea.Annotations.Add(telemetryChart.line);
            else
                telemetryChart.DefaultView.ChartArea.Annotations.Remove(telemetryChart.line);
        }
    }
}
