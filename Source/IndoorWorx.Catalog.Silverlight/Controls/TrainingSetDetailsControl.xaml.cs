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
        public TrainingSetDetailsControl()
        {
            InitializeComponent();
            IoC.Resolve<IEventAggregator>().GetEvent<TrainingSetSelectionChangedEvent>().Subscribe(TrainingSetSelectionChanged);
            IoC.Resolve<IEventAggregator>().GetEvent<VideoSelectionChangedEvent>().Subscribe(VideoSelectionChanged);
        }

        public void TrainingSetSelectionChanged(TrainingSet trainingSet)
        {
            LoadTelemetryOnChart(trainingSet);
        }

        public void VideoSelectionChanged(Video video)
        {
            if (video.SelectedTrainingSet != null)
                LoadTelemetryOnChart(video.SelectedTrainingSet);
        }

        private void profileChart_Loaded(object sender, RoutedEventArgs e)
        {
            var video = this.DataContext as TrainingSet;
            LoadTelemetryOnChart(video);
        }

        private void LoadTelemetryOnChart(TrainingSet video)
        {           
            if (video == null) return;
            this.DataContext = video;

            if (video.IsTelemetryLoaded)
            {
                profileChart.LoadTelemetry(video.Telemetry);
            }
            else
            {
                video.TelemetryLoaded += (_sender, _e) =>
                {
                    SmartDispatcher.BeginInvoke(() =>
                    {
                        profileChart.LoadTelemetry(video.Telemetry);
                    });
                };
            }
        }        
    }
}
