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
using IndoorWorx.Infrastructure.Models;
using System.Collections.Generic;
using System.Windows.Navigation;

namespace IndoorWorx.Infrastructure.Navigation
{
    public interface INavigationLinks
    {
        void Add(NavigationInfo navigation);

        bool Remove(NavigationInfo navigation);

        IEnumerable<NavigationInfo> All();

        void Clear();

        void MapUri(Uri from, Uri to);
    }
}
