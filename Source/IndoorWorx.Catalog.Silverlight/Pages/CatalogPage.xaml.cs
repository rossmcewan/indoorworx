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
using System.Windows.Navigation;
using IndoorWorx.Infrastructure;
using IndoorWorx.Catalog.Views;

namespace IndoorWorx.Catalog.Pages
{
    public partial class CatalogPage : Page
    {
        bool reloadRequired = true;
        public CatalogPage()
        {
            InitializeComponent();
            var contentElement = IoC.Resolve<ICatalogView>() as UserControl;
            if (contentElement.Parent != null)
            {
                (contentElement.Parent as CatalogPage).Content = null;
                reloadRequired = false;
            }
            this.Content = contentElement;
        }

        private ICatalogView View
        {
            get { return this.Content as ICatalogView; }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
