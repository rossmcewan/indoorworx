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
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Infrastructure.Navigation
{
    public interface INavigationService
    {
        void AddNavigationLink(NavigationInfo info);

        bool RemoveNavigationLink(NavigationInfo info);

        void NavigateTo(Uri uri);
    }
}
