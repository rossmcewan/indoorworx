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
using IndoorWorx.Designer.Models;
using IndoorWorx.Library.Controls;
using IndoorWorx.Infrastructure;

namespace IndoorWorx.Designer.Controls
{
    public partial class TrainingSetDesignControl : UserControl
    {
        public TrainingSetDesignControl()
        {
            InitializeComponent();
        }

        public TrainingSetDesign Model
        {
            set
            {
                this.DataContext = value;
            }
            get 
            { 
                return this.DataContext as TrainingSetDesign; 
            }
        }

        private void TelemetryChart_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTelemetryOnChart();
        }

        private void LoadTelemetryOnChart()
        {
            if (Model.Source.SelectedTrainingSet != null)
            {
                if (Model.Source.SelectedTrainingSet.IsTelemetryLoaded)
                    telemetryChart.LoadTelemetry(Model.Source.SelectedTrainingSet.Telemetry);
                else
                {
                    Model.Source.SelectedTrainingSet.TelemetryLoaded += (_sender, _e) =>
                        {
                            SmartDispatcher.BeginInvoke(() => telemetryChart.LoadTelemetry(Model.Source.SelectedTrainingSet.Telemetry));
                        };
                    Model.Source.SelectedTrainingSet.LoadTelemetry();
                }
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.OnTrainingSetSelectionChanged();
            LoadTelemetryOnChart();
        }
    }
}
