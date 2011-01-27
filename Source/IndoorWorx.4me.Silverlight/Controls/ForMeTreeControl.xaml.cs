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

namespace IndoorWorx.ForMe.Controls
{
    public partial class ForMeTreeControl : UserControl
    {
        public ForMeTreeControl()
        {
            InitializeComponent();
        }

        private IForMeTreeControlModel Model
        {
            get { return this.DataContext as IForMeTreeControlModel; }
        }

    }
}
