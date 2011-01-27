﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using IndoorWorx.Infrastructure;

namespace IndoorWorx.Library.Views
{
    public interface ITabbedNavigationPresentationModel<Tview> where Tview : ITabbedNavigationView
    {
        object SelectedItem { get; set; }

        Tview View { get; set; }

        bool IsBusy { get; set; }

        void AddNavigationItem(RadTreeViewItem navigationItem);

        void AddMainRegionView(IMainRegionView view);

    }
}
