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
using System.Windows.Threading;

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
        public Video Video
        {
            get
            {
                return video;
            }
            set
            {
                this.video = value;
                FirePropertyChanged("Video");
            }

        }

        private void LoadData()
        {
            var categoryService = serviceLocator.GetInstance<ICategoryService>();
            categoryService.CategoriesRetrieved += (sender, e) =>
            {
                var categories = e.Value;
                this.Video = categories.FirstOrDefault().Catalogs.FirstOrDefault().Videos.FirstOrDefault().TrainingSets.FirstOrDefault();
                this.Video.TelemetryLoaded += (_sender, _e) =>
                    {
                        SmartDispatcher.BeginInvoke(() =>
                        {
                            View.LoadVideo(Video);
                        });
                    };
                this.Video.LoadTelemetry();
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

        private void EnsurePlaying()
        {
            //if (player.CurrentState != SmoothStreamingMediaElementState.Playing && !ended)
            //    videoPlayer.Play();
        }


        public void MediaOpened()
        {
            //var timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(2);
            //timer.Tick += (_sender, _e) =>
            //{
            //    EnsurePlaying();
            //    var roundedSeconds = Math.Round(PlayerPosition.TotalSeconds);
            //    if (roundedSeconds % 2 == 0)
            //    {
            //        ChartData data = null;
            //        if (linked.TryGetValue(roundedSeconds, out data))
            //        {
            //            var val = data.YValue * 100;
            //            needle.Value = val;
            //            txtPower.Text = Math.Round(val).ToString();
            //        }
            //    }
            //    if (PlayerPosition >= lengthOfClip)
            //    {
            //        this.ended = true;
            //    }
            //    else
            //    {
            //        var xpos = new DateTime(now.Year, now.Month, now.Day, PlayerPosition.Hours, PlayerPosition.Minutes, PlayerPosition.Seconds).ToOADate();
            //        var rangeFrom = PlayerPosition.TotalSeconds / lengthOfClip.TotalSeconds;
            //        var rangeTo = rangeFrom + zoomedLength; //playerPosition.Add(TimeSpan.FromMinutes(3)).TotalSeconds / lengthOfClip.TotalSeconds;
            //        RadChart2.DefaultView.ChartArea.ZoomScrollSettingsX.SetSelectionRange(rangeFrom, rangeTo);
            //        line.XIntercept = xpos;
            //    }

            //    if (information.Count > 0 && (information.Peek().StartTime.TotalSeconds <= PlayerPosition.TotalSeconds))
            //    {
            //        ShowText(InformationQueue.Dequeue());
            //    }
            //};
            //timer.Start();
        }

        public void MediaEnded()
        {
            throw new NotImplementedException();
        }
    }
}
