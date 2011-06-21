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

namespace IndoorWorx.Catalog.Views
{
    public partial class VideoDetailsView : UserControl, IVideoDetailsView
    {
        public VideoDetailsView(IVideoDetailsPresentationModel model)
        {
            this.DataContext = model;
            InitializeComponent();
            model.View = this;
        }

        public IVideoDetailsPresentationModel Model
        {
            get { return this.DataContext as IVideoDetailsPresentationModel; }
        }        
    }
}