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
using Microsoft.Practices.Composite.Presentation.Commands;
using System.Threading;

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
            PlayCommand = new DelegateCommand<object>(Play);
            PauseCommand = new DelegateCommand<object>(Pause);
            FullScreenCommand = new DelegateCommand<object>(FullScreen);
        }

        private IShell Shell
        {
            get { return serviceLocator.GetInstance<IShell>(); }
        }

        private void Play(object arg)
        {
            View.Play();
            Video.IsPlaying = true;
            timer2.Start();
        }

        private void Pause(object arg)
        {
            timer2.Stop();
            Video.IsPlaying = false;
            View.Pause();
        }

        private void FullScreen(object arg)
        {
            Shell.IsFullScreen = true;
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

        private Queue<VideoText> textQueue = new Queue<VideoText>();

        private void LoadVideo(TrainingSet video)
        {
            foreach (var vt in video.VideoText.OrderBy(x => x.StartTime))
                textQueue.Enqueue(vt);
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

        private double zoomRangeFrom;
        public double ZoomRangeFrom
        {
            get { return zoomRangeFrom; }
            set
            {
                zoomRangeFrom = value;
                FirePropertyChanged("ZoomRangeFrom");
            }
        }

        private double zoomRangeTo;
        public double ZoomRangeTo
        {
            get { return zoomRangeTo; }
            set
            {
                zoomRangeTo = value;
                FirePropertyChanged("ZoomRangeTo");
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

        private DispatcherTimer timer2;
        public void MediaOpened()
        {
            timer2 = new DispatcherTimer();
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
                        var xpos = new DateTime(now.Year, now.Month, now.Day, PlayerPosition.Hours, PlayerPosition.Minutes, PlayerPosition.Seconds).ToOADate();
                        ZoomRangeFrom = PlayerPosition.TotalSeconds / Video.Duration.TotalSeconds;
                        ZoomRangeTo = ZoomRangeFrom + zoomedLength;
                    }
                    var tempA = Math.Round(PlayerPosition.TotalSeconds);
                    if (textQueue.Any() && textQueue.Peek().StartTime <= playerPosition)
                        LoadVideoText(textQueue.Dequeue());
                };
            Video.IsMediaLoading = false;
        }

        private void LoadVideoText(VideoText videoText)
        {
            View.AddTextAnimation(videoText);
        }

        private bool manifestReady;
        public bool IsManifestReady
        {
            get { return manifestReady; }
            set
            {
                manifestReady = value;
                FirePropertyChanged("IsManifestReady");
            }
        }

        private DateTime now = DateTime.Now;
        private double zoomedLength;
        public void ManifestReady()
        {
            zoomedLength = TimeSpan.FromMinutes(3).TotalSeconds / Video.Duration.TotalSeconds;
            ZoomRangeFrom = 0;
            ZoomRangeTo = zoomedLength;
            IsManifestReady = true;
        }

        public void MediaEnded()
        {
            this.hasVideoEnded = true;
        }

        private ICommand playCommand;
        public ICommand PlayCommand
        {
            get { return playCommand; }
            set
            {
                playCommand = value;
                FirePropertyChanged("PlayCommand");
            }
        }

        private ICommand pauseCommand;
        public ICommand PauseCommand
        {
            get { return pauseCommand; }
            set
            {
                pauseCommand = value;
                FirePropertyChanged("PauseCommand");
            }
        }

        private ICommand fullScreenCommand;
        public ICommand FullScreenCommand
        {
            get { return fullScreenCommand; }
            set
            {
                fullScreenCommand = value;
                FirePropertyChanged("FullScreenCommand");
            }
        }
    }
}
