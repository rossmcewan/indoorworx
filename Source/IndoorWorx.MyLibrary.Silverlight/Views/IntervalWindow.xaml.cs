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

namespace IndoorWorx.MyLibrary.Views
{
    public partial class IntervalWindow : ChildWindow, IIntervalView
    {
        public IntervalWindow(IIntervalPresentationModel model)
        {
            this.DataContext = model;
            InitializeComponent();
            model.View = this;            
        }

        public IIntervalPresentationModel Model
        {
            get { return this.DataContext as IIntervalPresentationModel; }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Model.OnAccepted(() => this.DialogResult = true);            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Model.OnCancelled(() => this.DialogResult = false);         
        }

        public void Hide()
        {
            this.Close();
        }
    }
}

