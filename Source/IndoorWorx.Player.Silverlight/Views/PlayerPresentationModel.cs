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
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Player.Views
{
    public class PlayerPresentationModel : BaseModel, IPlayerPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        public PlayerPresentationModel(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }


        private IPlayerView view;
        public IPlayerView View
        {
            get
            {
                return this.view;
            }
            set
            {
                this.view = value;
            }
        }
    }
}
