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
using IndoorWorx.Infrastructure;

namespace IndoorWorx.MyLibrary.Views
{
    public partial class TemplateView : UserControl, ITemplateView
    {
        private readonly IShell shell;
        public TemplateView(ITemplatePresentationModel model, IShell shell)
        {
            this.shell = shell;
            this.DataContext = model;
            InitializeComponent();
            model.View = this;
        }

        public ITemplatePresentationModel Model
        {
            get { return this.DataContext as ITemplatePresentationModel; }
        }
    }
}