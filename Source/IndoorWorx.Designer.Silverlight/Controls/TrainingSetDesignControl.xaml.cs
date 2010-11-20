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
using IndoorWorx.Designer.Domain;
using IndoorWorx.Library.Controls;

namespace IndoorWorx.Designer.Controls
{
    public partial class TrainingSetDesignControl : UserControl
    {
        public TrainingSetDesignControl()
        {
            InitializeComponent();
        }

        private TrainingSetDesign Model
        {
            get { return this.DataContext as TrainingSetDesign; }
        }

        private void TelemetryChart_Loaded(object sender, RoutedEventArgs e)
        {
            var chart = sender as TelemetryChart;
            chart.LoadTelemetry(Model.FromTrainingSet.Telemetry);
        }
    }
}
