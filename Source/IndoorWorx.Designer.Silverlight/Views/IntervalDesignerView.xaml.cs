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

namespace IndoorWorx.Designer.Views
{
    public partial class IntervalDesignerView : UserControl, IIntervalDesignerView
    {
        public IntervalDesignerView(IIntervalDesignerPresentationModel model)
        {
            this.DataContext = model;
            InitializeComponent();
            model.View = this; 
        }

        public IIntervalDesignerPresentationModel Model
        {
            get { return this.DataContext as IIntervalDesignerPresentationModel; }
        }
    }
}
