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
using IndoorWorx.Player.Resources;

namespace IndoorWorx.Player.Helpers
{
    public sealed class ResourceWrapper
    {
        private readonly PlayerResources playerResources = new PlayerResources();

        public PlayerResources PlayerResources
        {
            get { return playerResources; }
        }
    }
}
