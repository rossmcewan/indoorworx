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
        private Dictionary<double, Telemetry> linked = new Dictionary<double, Telemetry>();
        private Queue<Telemetry> queue = new Queue<Telemetry>();

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
                if (value != null && value is TrainingSet)
                {
                    LoadVideo((TrainingSet)value);
                }
                FirePropertyChanged("Video");
            }
        }

        private void LoadVideo(TrainingSet video)
        {
            if (video.IsTelemetryLoaded)
            {
                LoadLinkedDictionary();
                View.LoadTelemetry(video.Telemetry);
            }
            else
            {
                video.TelemetryLoaded -= video_TelemetryLoaded;
                video.TelemetryLoaded += video_TelemetryLoaded;
                video.LoadTelemetry();
            }
        }

        void LoadLinkedDictionary()
        {
            if (video is TrainingSet)
            {
                foreach (var t in (video as TrainingSet).Telemetry)
                {
                    linked.Add(t.TimePosition.TotalSeconds, t);
                    queue.Enqueue(t);
                }
            }
        }

        void video_TelemetryLoaded(object sender, EventArgs e)
        {
            var video = sender as TrainingSet;
            LoadLinkedDictionary();
            View.LoadTelemetry(video.Telemetry);
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
            View.EnsurePlaying();            
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

        private Telemetry currentTelemetry;
        public Telemetry CurrentTelemetry
        {
            get { return currentTelemetry; }
            set
            {
                currentTelemetry = value;
                FirePropertyChanged("CurrentTelemetry");
            }
        }

        public void MediaOpened()
        {
            var timer2 = new DispatcherTimer();
            timer2.Interval = TimeSpan.FromSeconds(1);
            timer2.Tick += (sender, e) =>
                {
                    if (queue.Peek().TimePosition <= PlayerPosition)
                        CurrentTelemetry = queue.Dequeue();
                    if (PlayerPosition >= Video.Duration)
                    {
                        View.EndVideo();
                        hasVideoEnded = true;
                    }
                    else
                    {
                        View.UpdateZoom(PlayerPosition);
                    }
                    //if (information.Count > 0 && (information.Peek().StartTime.TotalSeconds <= PlayerPosition.TotalSeconds))
                    //{
                    //    ShowText(InformationQueue.Dequeue());
                    //}
                };
            timer2.Start();
        }

        public void MediaEnded()
        {
            this.hasVideoEnded = true;
        }
    }
}
