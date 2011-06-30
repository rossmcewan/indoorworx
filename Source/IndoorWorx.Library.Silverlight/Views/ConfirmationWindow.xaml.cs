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
    public partial class ConfirmationWindow : ChildWindow
    {
        private Action<bool> closed;
        private ConfirmationWindow(Action<bool> closed)
        {
            this.closed = closed;
            InitializeComponent();
        }

        public static void Show(string confirmation, Action<bool> closed)
        {
            var window = new ConfirmationWindow(closed);
            window.confirmationText.Text = confirmation;
            window.Closed += (sender, e) =>
                {
                    closed(window.DialogResult.GetValueOrDefault());
                };
            window.Show();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

