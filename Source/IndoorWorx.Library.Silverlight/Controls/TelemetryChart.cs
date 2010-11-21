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
using IndoorWorx.Infrastructure.Helpers;

namespace IndoorWorx.Library.Controls
{
    public class TelemetryChart : RadChart
    {
        private CustomGridLine line;
        private MarkedZone zone;
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

            zone = new MarkedZone();
            this.DefaultView.ChartArea.Annotations.Add(zone);
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

        public ScrollMode ScrollSettingsX
        {
            get { return (ScrollMode)GetValue(ScrollSettingsXProperty); }
            set { SetValue(ScrollSettingsXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScrollSettingsX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollSettingsXProperty =
            DependencyProperty.Register("ScrollSettingsX", typeof(ScrollMode), typeof(TelemetryChart), new PropertyMetadata(ScrollMode.None, ScrollSettingsXChanged));

        private static void ScrollSettingsXChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = sender as TelemetryChart;
            chart.DefaultView.ChartArea.ZoomScrollSettingsX.ScrollMode = chart.ScrollSettingsX;
        }

        public ScrollMode ScrollSettingsY
        {
            get { return (ScrollMode)GetValue(ScrollSettingsYProperty); }
            set { SetValue(ScrollSettingsYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScrollSettingsY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollSettingsYProperty =
            DependencyProperty.Register("ScrollSettingsY", typeof(ScrollMode), typeof(TelemetryChart), new PropertyMetadata(ScrollMode.None, ScrollSettingsYChanged));

        private static void ScrollSettingsYChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = sender as TelemetryChart;
            chart.DefaultView.ChartArea.ZoomScrollSettingsY.ScrollMode = chart.ScrollSettingsY;
        }

        public Visibility YAxisMajorTickVisibility
        {
            get { return (Visibility)GetValue(YAxisMajorTickVisibilityProperty); }
            set { SetValue(YAxisMajorTickVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AxisYMajorTickVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YAxisMajorTickVisibilityProperty =
            DependencyProperty.Register("YAxisMajorTickVisibility", typeof(Visibility), typeof(TelemetryChart), new
                PropertyMetadata(Visibility.Visible, YAxisMajorTickVisibilityChanged));

        private static void YAxisMajorTickVisibilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = sender as TelemetryChart;
            chart.DefaultView.ChartArea.AxisY.MajorTicksVisibility = chart.YAxisMajorTickVisibility;
        }

        public Visibility YAxisMinorTickVisibility
        {
            get { return (Visibility)GetValue(YAxisMinorTickVisibilityProperty); }
            set { SetValue(YAxisMinorTickVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AxisYMinorTickVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YAxisMinorTickVisibilityProperty =
            DependencyProperty.Register("YAxisMinorTickVisibility", typeof(Visibility), typeof(TelemetryChart), new
                PropertyMetadata(Visibility.Visible, YAxisMinorTickVisibilityChanged));

        private static void YAxisMinorTickVisibilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = sender as TelemetryChart;
            chart.DefaultView.ChartArea.AxisY.MinorTicksVisibility = chart.YAxisMinorTickVisibility;
        }

        public Visibility XAxisMajorTickVisibility
        {
            get { return (Visibility)GetValue(XAxisMajorTickVisibilityProperty); }
            set { SetValue(XAxisMajorTickVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AxisXMajorTickVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XAxisMajorTickVisibilityProperty =
            DependencyProperty.Register("XAxisMajorTickVisibility", typeof(Visibility), typeof(TelemetryChart), new
                PropertyMetadata(Visibility.Visible, XAxisMajorTickVisibilityChanged));

        private static void XAxisMajorTickVisibilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = sender as TelemetryChart;
            chart.DefaultView.ChartArea.AxisX.MajorTicksVisibility = chart.XAxisMajorTickVisibility;
        }

        public Visibility XAxisMinorTickVisibility
        {
            get { return (Visibility)GetValue(XAxisMinorTickVisibilityProperty); }
            set { SetValue(XAxisMinorTickVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AxisXMinorTickVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XAxisMinorTickVisibilityProperty =
            DependencyProperty.Register("XAxisMinorTickVisibility", typeof(Visibility), typeof(TelemetryChart), new
                PropertyMetadata(Visibility.Visible, XAxisMinorTickVisibilityChanged));

        private static void XAxisMinorTickVisibilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = sender as TelemetryChart;
            chart.DefaultView.ChartArea.AxisX.MinorTicksVisibility = chart.XAxisMinorTickVisibility;
        }

        public double ZoneStartX
        {
            get { return (double)GetValue(ZoneStartXProperty); }
            set { SetValue(ZoneStartXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZoneStartX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoneStartXProperty =
            DependencyProperty.Register("ZoneStartX", typeof(double), typeof(TelemetryChart), new PropertyMetadata(0.0, ZoneStartXChanged));

        private static void ZoneStartXChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = sender as TelemetryChart;
            chart.zone.StartX = chart.ZoneStartX;
        }

        public double ZoneStartY
        {
            get { return (double)GetValue(ZoneStartYProperty); }
            set { SetValue(ZoneStartYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZoneStartY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoneStartYProperty =
            DependencyProperty.Register("ZoneStartY", typeof(double), typeof(TelemetryChart), new PropertyMetadata(0.0, ZoneStartYChanged));

        private static void ZoneStartYChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = sender as TelemetryChart;
            chart.zone.StartY = chart.ZoneStartY;
        }

        public double ZoneEndX
        {
            get { return (double)GetValue(ZoneEndXProperty); }
            set { SetValue(ZoneEndXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZoneEndX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoneEndXProperty =
            DependencyProperty.Register("ZoneEndX", typeof(double), typeof(TelemetryChart), new PropertyMetadata(0.0, ZoneEndXChanged));

        private static void ZoneEndXChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = sender as TelemetryChart;
            chart.zone.EndX = chart.ZoneEndX;
        }

        public double ZoneEndY
        {
            get { return (double)GetValue(ZoneEndYProperty); }
            set { SetValue(ZoneEndYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZoneEndY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoneEndYProperty =
            DependencyProperty.Register("ZoneEndY", typeof(double), typeof(TelemetryChart), new PropertyMetadata(0.0, ZoneEndYChanged));

        private static void ZoneEndYChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = sender as TelemetryChart;
            chart.zone.EndY = chart.ZoneEndY;
        }

        public Brush ZoneBackground
        {
            get { return (Brush)GetValue(ZoneBackgroundProperty); }
            set { SetValue(ZoneBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZoneBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoneBackgroundProperty =
            DependencyProperty.Register("ZoneBackground", typeof(Brush), typeof(TelemetryChart), new PropertyMetadata(null, ZoneBackgroundChanged));

        private static void ZoneBackgroundChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var chart = sender as TelemetryChart;
            chart.zone.Background = chart.ZoneBackground;
        }

        public void SetZoomScrollSettings(double rangeFrom,double rangeTo)
        {
            this.DefaultView.ChartArea.ZoomScrollSettingsX.SetSelectionRange(rangeFrom, rangeTo);    
        }

        public void Progress(double xInterceptPosition)
        {
            line.XIntercept = xInterceptPosition;
        }

        public void SetupAxisXRange(TimeSpan lengthOfClip)
        {
            this.DefaultView.ChartArea.AxisX.MinValue = DateTimeHelper.ZeroTime.ToOADate();
            this.DefaultView.ChartArea.AxisX.MaxValue = DateTimeHelper.ZeroTime.Add(lengthOfClip).ToOADate();
            this.DefaultView.ChartArea.AxisX.Step = 1.0 / 24.0 / 3600.0 / 2.0;
        }
    }
}
