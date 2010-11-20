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
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Player.Views
{
    public partial class PlayerView : UserControl ,IPlayerView
    {
        public PlayerView(IPlayerPresentationModel model)
        {
            InitializeComponent();            
            this.DataContext = model;
            model.View = this;
        }       

        public IPlayerPresentationModel Model
        {
            get { return this.DataContext as IPlayerPresentationModel; }
        }

        public void LoadVideo(Video video)
        {
            this.profileChart.LoadTelemetry(video.Telemetry);
            this.zoomedChart.LoadTelemetry(video.Telemetry);
        }
    }
}
