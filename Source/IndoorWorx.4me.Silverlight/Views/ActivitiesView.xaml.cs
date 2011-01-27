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
using IndoorWorx.Infrastructure;
using IndoorWorx.ForMe;

namespace IndoorWorx.ForMe.Views
{
    public partial class ActivitiesView : UserControl,IActivitiesView,IMainRegionView
    {
        public ActivitiesView()
        {
            InitializeComponent();
        }

        public string Header
        {
            get
            {
                return IndoorWorx.ForMe.Resources.ForMeResources.ActivitiesNavigationHeader;
            }
        }
    }
}
