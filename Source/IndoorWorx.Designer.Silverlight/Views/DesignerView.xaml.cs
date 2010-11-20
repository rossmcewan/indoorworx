﻿using System;
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

namespace IndoorWorx.Designer.Views
{
    public partial class DesignerView : UserControl, IDesignerView
    {
        private Storyboard playStory, stopStory;
        private DoubleAnimation playAnimation, stopAnimation;

        public DesignerView(IDesignerPresentationModel model)
        {
            InitializeComponent();
            this.DataContext = model;
            model.View = this;
            model.VideoSelected += new EventHandler<DataEventArgs<Video>>(model_VideoSelected);

            playStory = new Storyboard();
            playAnimation = new DoubleAnimation();
            playAnimation.From = 0.7;
            playAnimation.To = 1;
            playAnimation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetProperty(playAnimation, new PropertyPath("Opacity"));
            playStory.Children.Add(playAnimation);

            stopStory = new Storyboard();
            stopAnimation = new DoubleAnimation();
            stopAnimation.From = 1;
            stopAnimation.To = 0.7;
            stopAnimation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetProperty(stopAnimation, new PropertyPath("Opacity"));
            stopStory.Children.Add(stopAnimation);
        }

        void model_VideoSelected(object sender, DataEventArgs<Video> e)
        {
            var chart = this.profileChart;
            var video = e.Value;            
            if (video.IsTelemetryLoaded)
            {
                var max = video.Telemetry.Max(x => x.PercentageThreshold);
                chart.DefaultView.ChartArea.AxisY.AddRange(0, max, 0.2);
                chart.DefaultView.ChartArea.AxisX.MinValue = DateTime.Today.ToOADate();
                chart.DefaultView.ChartArea.AxisX.MaxValue = video.Telemetry.Max(x => x.TimePositionAsDateTime).ToOADate();
                chart.ItemsSource = video.Telemetry;
            }
            else
            {
                video.TelemetryLoaded += (_sender, _e) =>
                {
                    SmartDispatcher.BeginInvoke(() =>
                    {
                        var max = video.Telemetry.Max(x => x.PercentageThreshold);
                        chart.DefaultView.ChartArea.AxisY.AddRange(0, max, 0.2);
                        chart.DefaultView.ChartArea.AxisX.MinValue = DateTime.Today.ToOADate();
                        chart.DefaultView.ChartArea.AxisX.MaxValue = video.Telemetry.Max(x => x.TimePositionAsDateTime).ToOADate();
                        chart.ItemsSource = video.Telemetry;
                    });
                };
            }
        }

        public IDesignerPresentationModel Model
        {
            get { return this.DataContext as IDesignerPresentationModel; }
        }

        public void AddDesigner()
        {
            DocumentPaneGroup.AddItem(new RadDocumentPane() { Title = Designer.Resources.DesignerResources.NewDesignTitle }, DockPosition.Center);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var player = GetPlayer();
            playStory.Stop();
            Storyboard.SetTarget(playAnimation, player);
            Model.PlaySelectedPreview(() =>
            {
                playStory.Begin();
                player.Play();
            });
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            var player = GetPlayer();
            stopStory.Stop();
            Storyboard.SetTarget(stopAnimation, player);
            Model.StopSelectedPreview(() =>
            {
                stopStory.Begin();
                player.Stop();
            });
        }

        private SmoothStreamingMediaElement GetPlayer()
        {
            return mediaElement;
        }

        private void profileChart_Loaded(object sender, RoutedEventArgs e)
        {
            var chart = sender as RadChart;
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

            chart.DefaultView.ChartArea.ZoomScrollSettingsX.ScrollMode = ScrollMode.None;
            chart.DefaultView.ChartArea.ZoomScrollSettingsY.ScrollMode = ScrollMode.None;

            chart.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisX.AxisLabelsVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisY.AxisLabelsVisibility = Visibility.Collapsed;

            chart.DefaultView.ChartArea.AxisY.AutoRange = false;
            chart.DefaultView.ChartArea.AxisY.MajorGridLinesVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisY.MinorGridLinesVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisY.StripLinesVisibility = Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = Visibility.Collapsed;

            chart.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;
            chart.SamplingSettings.SamplingThreshold = 1000;
            chart.DefaultView.ChartArea.EnableAnimations = false;            
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var video = fe.DataContext as Video;
            if (video != null)
            {
                video.IsMediaLoading = false;
            }
        }

        private void mediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void mediaElement_SmoothStreamingErrorOccurred(object sender, SmoothStreamingErrorEventArgs e)
        {

        }

        private void profileChart_DataBound(object sender, ChartDataBoundEventArgs e)
        {

        }

        private void profileChart_DataBinding(object sender, ChartDataBindingEventArgs e)
        {

        }
    }
}
