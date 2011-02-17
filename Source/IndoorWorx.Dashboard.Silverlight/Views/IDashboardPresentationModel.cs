using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Dashboard.Views
{
    public interface IDashboardPresentationModel
    {
        IDashboardView View { get; set; }

        ICollection<Widget> AvailableWidgets { get; set; }

        ICollection<Widget> AddedWidgets { get; set; }

        void AddWidget(Widget widget);

        void Refresh();
    }
}
