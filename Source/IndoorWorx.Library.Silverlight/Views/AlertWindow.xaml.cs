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

namespace IndoorWorx.Library.Views
{
    public partial class AlertWindow : ChildWindow
    {
        private AlertWindow()
        {
            InitializeComponent();
        }

        public static void Show(string alert)
        {
            var window = new AlertWindow();
            window.alertText.Text = alert;
            window.Show();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

