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
using Telerik.Windows.Controls.DragDrop;
using IndoorWorx.MyLibrary.Helpers;

namespace IndoorWorx.MyLibrary.Views
{
    public partial class TemplatesView : UserControl, ITemplatesView
    {
        public TemplatesView(ITemplatesPresentationModel model)
        {
            this.DataContext = model;            
            InitializeComponent();
            model.View = this;
        }

        public ITemplatesPresentationModel Model
        {
            get { return this.DataContext as ITemplatesPresentationModel; }
        }        
    }
}
