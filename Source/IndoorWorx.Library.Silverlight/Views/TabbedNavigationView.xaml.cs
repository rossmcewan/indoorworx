﻿using System;
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
    public partial class TabbedNavigationView : UserControl, ITabbedNavigationView
    {
        public TabbedNavigationView(ITabbedNavigationPresentationModel model)
        {
            InitializeComponent();
            this.DataContext = model;
            model.View = this;
        }

        public ITabbedNavigationPresentationModel Model
        {
            get { return this.DataContext as ITabbedNavigationPresentationModel; }
        }

        private void RadDocking_PreviewClose(object sender, Telerik.Windows.Controls.Docking.StateChangeEventArgs e)
        {

        }

        private void RadDocking_Close(object sender, Telerik.Windows.Controls.Docking.StateChangeEventArgs e)
        {

        }

        private void navigationTreeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // Model.NavigationItemSelectionChanged(e.OriginalSource);
        }
    }
}
