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

namespace IndoorWorx.Trainers.Views
{
    public partial class SelectTrainerWindow : ChildWindow, ISelectTrainerView
    {
        public SelectTrainerWindow(ISelectTrainerPresentationModel model)
        {
            this.DataContext = model;
            InitializeComponent();
        }

        public ISelectTrainerPresentationModel Model
        {
            get { return this.DataContext as ISelectTrainerPresentationModel; }
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
