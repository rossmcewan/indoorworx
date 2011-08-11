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
using IndoorWorx.Library.Controls;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Catalog.Events;

namespace IndoorWorx.Catalog.Controls
{
    public partial class TrainingSetDetailsControl : UserControl
    {
        private static readonly List<Telemetry> EmptyTelemetry = new List<Telemetry>();

        public TrainingSetDetailsControl()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(TrainingSetDetailsControl_Loaded);
            this.Unloaded += new RoutedEventHandler(TrainingSetDetailsControl_Unloaded);            
        }

        void TrainingSetDetailsControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //IoC.Resolve<IEventAggregator>().GetEvent<TrainingSetSelectionChangedEvent>().Unsubscribe(TrainingSetSelectionChanged);
            IoC.Resolve<IEventAggregator>().GetEvent<VideoSelectionChangedEvent>().Unsubscribe(VideoSelectionChanged);
        }

        void TrainingSetDetailsControl_Loaded(object sender, RoutedEventArgs e)
        {
            //IoC.Resolve<IEventAggregator>().GetEvent<TrainingSetSelectionChangedEvent>().Subscribe(TrainingSetSelectionChanged);
            IoC.Resolve<IEventAggregator>().GetEvent<VideoSelectionChangedEvent>().Subscribe(VideoSelectionChanged);
        }

        //public void TrainingSetSelectionChanged(TrainingSet trainingSet)
        //{
        //    LoadTelemetryOnChart(trainingSet);
        //}

        public void VideoSelectionChanged(Video video)
        {
            //if (video != null && video.SelectedTrainingSet != null)
            //{
            //    LoadTelemetryOnChart(video.SelectedTrainingSet);
            //}
            //else
            //{
            //    ClearTelemetryChart();
            //}
        }

        private void profileChart_Loaded(object sender, RoutedEventArgs e)
        {
            //var video = this.DataContext as TrainingSet;
            //LoadTelemetryOnChart(video);
        }

        private void ClearTelemetryChart()
        {
            //SmartDispatcher.BeginInvoke(() => profileChart.LoadTelemetry(EmptyTelemetry));
        }

        private void LoadTelemetryOnChart(TrainingSet video)
        {           
            //if (video == null) return;
            //this.DataContext = video;

            //if (video.IsTelemetryLoaded)
            //{
            //    SmartDispatcher.BeginInvoke(() => profileChart.LoadTelemetry(video.Telemetry));
            //}
            //else
            //{
            //    video.TelemetryLoaded += (_sender, _e) =>
            //    {
            //        SmartDispatcher.BeginInvoke(() =>
            //        {
            //            profileChart.LoadTelemetry(video.Telemetry);
            //        });
            //    };
            //}
        }        
    }
}
