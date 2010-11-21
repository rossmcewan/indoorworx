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

namespace IndoorWorx.Catalog.Controls
{
    public partial class TrainingSetDetailsControl : UserControl
    {
        public TrainingSetDetailsControl()
        {
            InitializeComponent();
        }

        private void profileChart_Loaded(object sender, RoutedEventArgs e)
        {
            var chart = sender as TelemetryChart;
            var video = chart.DataContext as Video;

            if (video.IsTelemetryLoaded)
            {
                chart.LoadTelemetry(video.Telemetry);
            }
            else
            {
                video.TelemetryLoaded += (_sender, _e) =>
                {
                    SmartDispatcher.BeginInvoke(() =>
                    {
                        chart.LoadTelemetry(video.Telemetry);
                    });
                };
            }
        }
    }
}
