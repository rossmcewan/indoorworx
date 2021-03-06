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
using IndoorWorx.Infrastructure.DragDrop;

namespace IndoorWorx.MyLibrary.Views
{
    public interface ITemplatesPresentationModel
    {
        ITemplatesView View { set; get; }

        ICommand AddTemplateCommand { get; }

        ICommand EditTemplateCommand { get; }

        ICommand RemoveTemplateCommand { get; }

        ICommand CreateRideCommand { get; }

        void Refresh();
    }
}
