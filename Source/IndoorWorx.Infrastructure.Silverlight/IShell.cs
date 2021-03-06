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

namespace IndoorWorx.Infrastructure
{
    public interface IShell
    {        
        void Show();

        bool IsFullScreen { get; set; }

        void AddToLayoutRoot(UIElement ui);

        void RemoveFromLayoutRoot(UIElement ui);
    }
}
