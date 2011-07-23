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

namespace IndoorWorx.Player.Views
{
    public partial class PlayerDataCaptureWindow : ChildWindow, IPlayerDataCaptureView
    {
        public PlayerDataCaptureWindow(IPlayerDataCapturePresentationModel model)
        {
            this.DataContext = model;
            InitializeComponent();
            model.View = this;
        }

        public IPlayerDataCapturePresentationModel Model
        {
            get { return this.DataContext as IPlayerDataCapturePresentationModel; }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public void Hide()
        {
            this.Close();
        }

        public void Show(Action ok, Action cancel)
        {
            this.Closed += (sender, e) =>
                {
                    if (this.DialogResult.GetValueOrDefault())
                        ok();
                    else
                        cancel();
                };
            Show();
        }

        private void RadSlider_DragCompleted(object sender, Telerik.Windows.Controls.RadDragCompletedEventArgs e)
        {
            Model.Video.LoadPlayingTelemetry();
        }
    }
}

