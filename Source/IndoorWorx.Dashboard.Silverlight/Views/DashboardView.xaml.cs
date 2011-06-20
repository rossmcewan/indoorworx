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

namespace IndoorWorx.Dashboard.Views
{
    public partial class DashboardView : UserControl, IDashboardView
    {
        public DashboardView(IDashboardPresentationModel model)
        {
            this.DataContext = model;
            model.View = this;
            InitializeComponent();
        }

        public IDashboardPresentationModel Model
        {
            get { return this.DataContext as IDashboardPresentationModel; }
        }        
    }
}
