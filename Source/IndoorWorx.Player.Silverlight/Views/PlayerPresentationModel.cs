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
using Microsoft.Web.Media.SmoothStreaming;

namespace IndoorWorx.Player.Views
{
    public class PlayerPresentationModel : BaseModel, IPlayerPresentationModel
    {
        private bool hasVideoEnded = false;
        private readonly IServiceLocator serviceLocator;

        public PlayerPresentationModel(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
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
                if (value != null)
                {
                    View.LoadVideo(value);
                }
                FirePropertyChanged("Video");
            }

        }


        private SmoothStreamingMediaElement Player
        {
            get { return View.GetPlayer(); }
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

        private TimeSpan lengthOfClip;
        public TimeSpan LengthOfClip
        {
            get { return lengthOfClip; }
            set
            {
                lengthOfClip = value;
                FirePropertyChanged("LengthOfClip");
            }
        }

        private void EnsurePlaying()
        {
            if (Player.CurrentState != SmoothStreamingMediaElementState.Playing && !hasVideoEnded)
                Player.Play();
        }

        private double currentIntensity = 0;
        public double CurrentIntensity
        {
            get { return currentIntensity; }
            set
            {
                currentIntensity = value;
                FirePropertyChanged("CurrentIntensity");
            }
        }


        public void MediaOpened()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (_sender, _e) =>
            {
                EnsurePlaying();
                var roundedSeconds = Math.Round(PlayerPosition.TotalSeconds);
                //if (roundedSeconds % 3 == 0)
                //{
                 Telemetry data = Video.Telemetry.Where(t => Math.Round(t.TimePosition.TotalSeconds) == roundedSeconds).FirstOrDefault();
                    if (data != null)
                    {
                        this.CurrentIntensity = Math.Round(data.PercentageThreshold*100);
                    }
               // }
                if (PlayerPosition >= LengthOfClip)
                {
                    hasVideoEnded = true;
                }
                else
                {
                    View.UpdateCurrentPosition(PlayerPosition);

                }
                //if (information.Count > 0 && (information.Peek().StartTime.TotalSeconds <= PlayerPosition.TotalSeconds))
                //{
                //    ShowText(InformationQueue.Dequeue());
                //}
            };
            timer.Start();
        }

        public void MediaEnded()
        {
            this.hasVideoEnded = true;
        }

    }
}
