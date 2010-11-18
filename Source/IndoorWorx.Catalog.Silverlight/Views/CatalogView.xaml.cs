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
        private Storyboard playStory, stopStory;
        private DoubleAnimation playAnimation, stopAnimation;

        public CatalogView(ICatalogPresentationModel model)
        {
            InitializeComponent();
            this.DataContext = model;
            model.View = this;

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

        #region ICatalogView Members

        public ICatalogPresentationModel Model
        {
            get { return this.DataContext as ICatalogPresentationModel; }
        }

        #endregion

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
            var item = radTileView.ItemContainerGenerator.ContainerFromItem(Model.SelectedCategory.SelectedCatalog.SelectedVideo ?? Model.SelectedCategory.SelectedCatalog.Videos.FirstOrDefault());

            var player = FindVisualChild<SmoothStreamingMediaElement>(item);

            return player;
        }

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
