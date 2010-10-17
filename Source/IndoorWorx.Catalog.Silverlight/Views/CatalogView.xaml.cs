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
    public partial class CatalogView : UserControl, ICatalogView
    {
        public CatalogView(ICatalogPresentationModel model)
        {
            InitializeComponent();
            this.DataContext = model;
            model.View = this;
        }

        #region ICatalogView Members

        public ICatalogPresentationModel Model
        {
            get { return this.DataContext as ICatalogPresentationModel; }
        }

        #endregion
    }
}
