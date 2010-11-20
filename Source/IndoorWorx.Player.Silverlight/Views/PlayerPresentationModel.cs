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
using System.Windows.Browser;
using System.IO;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Services;
using System.Linq;
using IndoorWorx.Infrastructure;

namespace IndoorWorx.Player.Views
{
    public class PlayerPresentationModel : BaseModel, IPlayerPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        public PlayerPresentationModel(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            LoadData();
           
        }


        private Video video = null;

        private void LoadData()
        {
            var categoryService = serviceLocator.GetInstance<ICategoryService>();
            categoryService.CategoriesRetrieved += (sender, e) =>
            {
                var categories = e.Value;
                this.video = categories.FirstOrDefault().Catalogs.FirstOrDefault().Videos.FirstOrDefault().TrainingSets.FirstOrDefault();
                this.video.TelemetryLoaded += (_sender, _e) =>
                    {
                        SmartDispatcher.BeginInvoke(() =>
                        {
                            View.LoadVideo(video);
                        });
                    };
            };
            categoryService.RetrieveCategories();
        }



        private TimeSpan playerPosition = new TimeSpan();
        public TimeSpan PlayerPosition
        {
            get { return playerPosition; }
            set
            {
                playerPosition = value;
                FirePropertyChanged("PlayerPosition");
            }

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

        private VideoText currentVideoText;
        public VideoText CurrentVideoText
        {
            get { return currentVideoText; }
            set
            {
                currentVideoText = value;
                FirePropertyChanged("CurrentVideoText");
            }
        }
    }
}
