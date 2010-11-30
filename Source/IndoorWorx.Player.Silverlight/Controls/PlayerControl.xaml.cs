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
using System.Windows.Threading;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Charting;
using System.IO;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Player.Controls
{
    public partial class PlayerControl : UserControl
    {
        DateTime zeroTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

        public PlayerControl()
        {
            InitializeComponent();
            ConfigureCharts();
        }

        private Dictionary<double, Telemetry> linked = new Dictionary<double, Telemetry>();

        public void BindChart(ICollection<Telemetry> telemetry)
        {
            foreach (var t in telemetry)
            {
                linked.Add(t.TimePosition.TotalSeconds, t);
            }

            RadChart1.ItemsSource = telemetry;
            RadChart2.ItemsSource = telemetry;

            var max = telemetry.Max(x => x.PercentageThreshold);
            RadChart1.DefaultView.ChartArea.AxisY.AddRange(0, max, 0.2);
            RadChart2.DefaultView.ChartArea.AxisY.AddRange(0, max, 0.2);
            radialScale.Min = 0;
            radialScale.MajorTickStep = 20;
            radialScale.Max = max * 100;
            radialScale.Label.Location = Telerik.Windows.Controls.Gauges.ScaleObjectLocation.Outside;
        }                

        CustomGridLine line = new CustomGridLine();
        private void ConfigureCharts()
        {
            ConfigureChart(RadChart1);            
            line.XIntercept = 0;
            line.Visibility = System.Windows.Visibility.Visible;
            line.Stroke = new SolidColorBrush(Colors.Green);
            line.StrokeThickness = 2;
            RadChart1.DefaultView.ChartArea.AxisY.AutoRange = false;
            RadChart1.DefaultView.ChartArea.Annotations.Add(line);

            ConfigureChart(RadChart2);
            RadChart2.DefaultView.ChartArea.AxisY.AutoRange = false;
        }

        private void ConfigureChart(RadChart chart)
        {            
            AreaSeriesDefinition lineSeries = new AreaSeriesDefinition();
            lineSeries.ShowItemLabels = false;
            lineSeries.ShowPointMarks = false;

            SeriesMapping dataMapping = new SeriesMapping();
            dataMapping.SeriesDefinition = lineSeries;
            dataMapping.ItemMappings.Add(new ItemMapping("TimePositionAsDateTime", DataPointMember.XValue));
            dataMapping.ItemMappings.Add(new ItemMapping("PercentageThreshold", DataPointMember.YValue));
            dataMapping.ItemMappings[1].SamplingFunction = ChartSamplingFunction.KeepExtremes;

            chart.SeriesMappings.Add(dataMapping);

            chart.DefaultView.ChartArea.ZoomScrollSettingsX.ScrollMode = ScrollMode.None;
            chart.DefaultView.ChartArea.ZoomScrollSettingsY.ScrollMode = ScrollMode.None;

            chart.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisX.DefaultLabelFormat = "#VAL{HH:mm:ss}";
            chart.DefaultView.ChartArea.AxisY.DefaultLabelFormat = "#VAL{p0}";
            chart.AxisElementBrush = new SolidColorBrush(Colors.White);
            chart.AxisForeground = new SolidColorBrush(Colors.White);
            chart.DefaultView.ChartArea.AxisY.MajorGridLinesVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisY.MinorGridLinesVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisY.StripLinesVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;
            chart.SamplingSettings.SamplingThreshold = 1000;
            chart.DefaultView.ChartArea.EnableAnimations = false;
        }

        private void videoPlayer_ClipError(object sender, Microsoft.Web.Media.SmoothStreaming.ClipEventArgs e)
        {

        }

        private void videoPlayer_SmoothStreamingErrorOccurred(object sender, Microsoft.Web.Media.SmoothStreaming.SmoothStreamingErrorEventArgs e)
        {

        }

        private void videoPlayer_ClipStateChanged(object sender, Microsoft.Web.Media.SmoothStreaming.ClipEventArgs e)
        {

        }

        private void videoPlayer_CurrentStateChanged(object sender, RoutedEventArgs e)
        {

        }

        private TimeSpan lengthOfClip;
        private double zoomedLength;
        private DateTime now = DateTime.Now;
        private void videoPlayer_ManifestReady(object sender, EventArgs e)
        {
            lengthOfClip = videoPlayer.EndPosition;
            zoomedLength = TimeSpan.FromMinutes(3).TotalSeconds / lengthOfClip.TotalSeconds;
            SetupAxisXRange(RadChart1, lengthOfClip);
            SetupAxisXRange(RadChart2, lengthOfClip);
            RadChart2.DefaultView.ChartArea.ZoomScrollSettingsX.SetSelectionRange(0, zoomedLength);
        }

        private void SetupAxisXRange(RadChart chart, TimeSpan lengthOfClip)
        {            
            chart.DefaultView.ChartArea.AxisX.MinValue = zeroTime.ToOADate();
            chart.DefaultView.ChartArea.AxisX.MaxValue = zeroTime.Add(lengthOfClip).ToOADate();
            chart.DefaultView.ChartArea.AxisX.Step = 1.0 / 24.0 / 3600.0 / 2.0;
        }

        private void videoPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (_sender, _e) =>
                {
                    EnsurePlaying();
                    var playerPosition = videoPlayer.Position;
                    var roundedSeconds = Math.Round(playerPosition.TotalSeconds);
                    
                    Telemetry data = null;
                    if (linked.TryGetValue(roundedSeconds, out data))
                        needle.Value = data.PercentageThreshold * 100;
                    
                    if (playerPosition >= lengthOfClip)
                    {
                        this.ended = true;
                    }
                    else
                    {
                        var xpos = new DateTime(now.Year, now.Month, now.Day, playerPosition.Hours, playerPosition.Minutes, playerPosition.Seconds).ToOADate();
                        var rangeFrom = playerPosition.TotalSeconds / lengthOfClip.TotalSeconds;
                        var rangeTo = rangeFrom + zoomedLength; //playerPosition.Add(TimeSpan.FromMinutes(3)).TotalSeconds / lengthOfClip.TotalSeconds;
                        RadChart2.DefaultView.ChartArea.ZoomScrollSettingsX.SetSelectionRange(rangeFrom, rangeTo);
                        line.XIntercept = xpos;
                    }
                };
            timer.Start();
        }

        private void EnsurePlaying()
        {
            //if (videoPlayer.CurrentState != SmoothStreamingMediaElementState.Playing && !ended)
            //    videoPlayer.Play();
        }

        private bool ended = false;
        private void videoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.ended = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (videoPlayer.CurrentState != SmoothStreamingMediaElementState.Playing)
                videoPlayer.Play();
            else
                videoPlayer.Pause();
        }
    }
}
