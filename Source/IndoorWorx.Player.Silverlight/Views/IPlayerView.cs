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
using Microsoft.Web.Media.SmoothStreaming;

namespace IndoorWorx.Player.Views
{
    public interface IPlayerView
    {
        IPlayerPresentationModel Model { get; }

        void LoadVideo(Video video);

        void UpdateCurrentPosition(TimeSpan PlayerPosition);

        SmoothStreamingMediaElement GetPlayer();
    }
}
