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
using Telerik.Windows.Controls.Charting;
using System.IO;
using Telerik.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Web.Media.SmoothStreaming;

namespace VideoTelemetry
{
    public partial class MainPage : UserControl
    {
        DateTime zeroTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

        public MainPage()
        {
            InitializeComponent();
            ConfigureCharts();
            FillSampleChartData();
        }

        private void FillSampleChartData()
        {
            Uri dataURI = new Uri(System.Windows.Browser.HtmlPage.Document.DocumentUri, "DataSources/telemetry.csv");
            System.Net.WebClient dataRetriever = new System.Net.WebClient();
            dataRetriever.DownloadStringCompleted += new System.Net.DownloadStringCompletedEventHandler(dataRetriever_DownloadStringCompleted);
            dataRetriever.DownloadStringAsync(dataURI);
        }

        private Dictionary<double, ChartData> linked = new Dictionary<double, ChartData>();

        private void BindChart(TextReader dataReader)
        {
            string line;
            int count = 0;

            List<ChartData> chartData = new List<ChartData>();

            while ((line = dataReader.ReadLine()) != null)
            {
                if (count == 0)
                {
                    count++;
                    continue;
                }
                var elements = line.Split(',');
                var minutes = TimeSpan.FromMinutes(Convert.ToDouble(elements[0]));
                var chartDataElement = new ChartData(zeroTime.Add(minutes), double.Parse(elements[3])/300.0);
                chartData.Add(chartDataElement);
                linked.Add(minutes.TotalSeconds, chartDataElement);
            }

            RadChart1.ItemsSource = chartData;
            RadChart2.ItemsSource = chartData;

            var max = chartData.Max(x=>x.YValue);
            RadChart1.DefaultView.ChartArea.AxisY.AddRange(0, max, 0.2);
            RadChart2.DefaultView.ChartArea.AxisY.AddRange(0, max, 0.2);
            radialScale.Min = 0;
            radialScale.MajorTickStep = 20;
            radialScale.Max = max*100;
            radialScale.Label.Location = Telerik.Windows.Controls.Gauges.ScaleObjectLocation.Outside;
        }

        private void dataRetriever_DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            StringReader dataReader = new StringReader(e.Result);
            this.BindChart(dataReader);
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
            dataMapping.ItemMappings.Add(new ItemMapping("XValue", DataPointMember.XValue));
            dataMapping.ItemMappings.Add(new ItemMapping("YValue", DataPointMember.YValue));
            dataMapping.ItemMappings[1].SamplingFunction = ChartSamplingFunction.KeepExtremes;

            chart.SeriesMappings.Add(dataMapping);

            chart.DefaultView.ChartArea.ZoomScrollSettingsX.ScrollMode = ScrollMode.None;
            chart.DefaultView.ChartArea.ZoomScrollSettingsY.ScrollMode = ScrollMode.None;

            chart.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisX.DefaultLabelFormat = "#VAL{HH:mm:ss}";
            chart.DefaultView.ChartArea.AxisY.DefaultLabelFormat = "#VAL{p0}";
            //chart.DefaultView.ChartArea.AxisX.LabelRotationAngle = 90;
            //chart.DefaultView.ChartArea.AxisY.AutoRange = true;
            //chart.DefaultView.ChartArea.AxisY.AddRange(0, 6000, 500);
            //chart.DefaultView.ChartArea.AxisX.DefaultLabelFormat = "#VAL{0.}";

            chart.AxisElementBrush = new SolidColorBrush(Colors.White);
            chart.AxisForeground = new SolidColorBrush(Colors.White);
            chart.DefaultView.ChartArea.AxisY.MajorGridLinesVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisY.MinorGridLinesVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisY.StripLinesVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = Visibility.Collapsed;
            //chart.DefaultView.ChartArea.Padding = new Thickness(5, 10, 20, 5);

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
                    if (roundedSeconds % 2 == 0)
                    {
                        ChartData data = null;
                        if(linked.TryGetValue(roundedSeconds, out data))
                            needle.Value = data.YValue*100;
                    }
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
            if (videoPlayer.CurrentState != SmoothStreamingMediaElementState.Playing && !ended)
                videoPlayer.Play();
        }

        private bool ended = false;
        private void videoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.ended = true;
        }
    }

    public class ChartData
    {
        private DateTime _xValue;
        private double _yValue;

        public DateTime XValue
        {
            get
            {
                return this._xValue;
            }
            set
            {
                this._xValue = value;
            }
        }

        public double YValue
        {
            get
            {
                return this._yValue;
            }
            set
            {
                this._yValue = value;
            }
        }

        public ChartData(DateTime xValue, double yValue)
        {
            this.XValue = xValue;
            this.YValue = yValue;
        }
    }
}
