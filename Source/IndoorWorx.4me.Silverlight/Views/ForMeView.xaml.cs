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

namespace IndoorWorx.ForMe.Views
{
    public partial class ForMeView : UserControl, IForMeView
    {
        public ForMeView(IForMePresentationModel model)
        {
            InitializeComponent();
            this.DataContext = model;
            model.View = this;
        }

        public IForMePresentationModel Model
        {
            get { return this.DataContext as IForMePresentationModel; }
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
