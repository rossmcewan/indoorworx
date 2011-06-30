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

namespace IndoorWorx.Silverlight.Views
{
    public partial class UpdateAvailableWindow : ChildWindow
    {
        private UpdateAvailableWindow(Action closed)
        {            
            InitializeComponent();
            this.Closed += (sender, e) =>
                {
                    closed();
                };
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public static void Show(Action closed)
        {
            var window = new UpdateAvailableWindow(closed);

            window.Show();
        }
    }
}

