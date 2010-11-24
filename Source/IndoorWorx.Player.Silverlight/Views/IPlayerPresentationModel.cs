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

namespace IndoorWorx.Player.Views
{
    public interface IPlayerPresentationModel
    {
        IPlayerView View { get; set; }

        Video Video { get; set; }

        void MediaOpened();

        void MediaEnded();

        void ManifestReady();

        ICommand PlayCommand { get;set;}

        ICommand PauseCommand { get; set; }

        ICommand FullScreenCommand { get; set; }
    }
}
