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

namespace RCE.Modules.Telemetry.Views
{
    public partial class TelemetryView : UserControl
    {
        public TelemetryView()
        {
            InitializeComponent();
            var c = new System.Windows.Controls.DataVisualization.Charting.Chart();            
        }
    }
}
