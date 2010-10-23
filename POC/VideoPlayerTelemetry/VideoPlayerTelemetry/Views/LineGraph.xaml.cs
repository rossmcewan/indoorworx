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
using System.Windows.Controls.DataVisualization.Charting;

namespace VideoPlayerTelemetry.Views
{
    public partial class LineGraph : UserControl
    {
        public LineGraph()
        {
            InitializeComponent();

            //var dtAxis = powerChart.Axes[1] as DateTimeAxis;
            //if (dtAxis != null)
            //{
            //    dtAxis.Interval = 5;
            //    dtAxis.IntervalType = DateTimeIntervalType.Minutes;
            //}
        }

        private void powerChart_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void LineSeries_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void dateTimeAxis_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
