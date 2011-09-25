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
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Facades;
using IndoorWorx.Infrastructure.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using IndoorWorx.Infrastructure.Helpers;

namespace IndoorWorx.Player.Views
{
    public class PlayerPresentationModel : BaseModel, IPlayerPresentationModel
    {
        private bool hasVideoEnded = false;
        private readonly IServiceLocator serviceLocator;
        private Dictionary<double, Telemetry> linked = new Dictionary<double, Telemetry>();
        private Queue<Telemetry> queue = new Queue<Telemetry>();
        private IEventAggregator eventAggregator;

        public PlayerPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            PlayCommand = new DelegateCommand<object>(Play);
            StopCommand = new DelegateCommand<object>(Stop);
            PauseCommand = new DelegateCommand<object>(Pause);
            FullScreenCommand = new DelegateCommand<object>(FullScreen);
            ExportTrainerFileCommand = new DelegateCommand<object>(ExportTrainerFile);
        }

        private void ExportTrainerFile(object arg)
        {
            eventAggregator.GetEvent<ExportTrainerFileEvent>().Publish(Video.Telemetry);
        }

        private IShell Shell
        {
            get { return serviceLocator.GetInstance<IShell>(); }
        }

        private IDialogFacade DialogFacade
        {
            get { return serviceLocator.GetInstance<IDialogFacade>(); }
        }

        Timer timer;
        private void Play(object arg)
        {
            timer = new Timer(new TimerCallback(Countdown), new Action(() =>
                {
                    View.Play();
                    Video.IsPlaying = true;
                    StartTimers();
                }), TimeSpan.Zero, TimeSpan.FromSeconds(1));
            //for (int i = 1; i <= 5; i++)
            //{
            //    View.AddTextAnimation(new VideoText() { MainText = i.ToString(), Duration = TimeSpan.FromSeconds(1), Animation = Infrastructure.Enums.VideoTextAnimations.ZoomCenter });
            //    //Thread.Sleep(1000);
            //}            
            //View.Play();
            //Video.IsPlaying = true;
            //StartTimers();
        }

        private int counter = 0;
        private const int countFrom = 6;
        private void Countdown(object arg)
        {
            counter++;
            if (counter > 5 || paused)
            {
                paused = false;
                var play = arg as Action;
                counter = 0;
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                SmartDispatcher.BeginInvoke(() => play());
            }
            else
            {
                SmartDispatcher.BeginInvoke(() => View.AddTextAnimation(new VideoText() { MainText = (countFrom - counter).ToString(), Duration = TimeSpan.FromSeconds(1), Animation = Infrastructure.Enums.VideoTextAnimations.ZoomCenter }));
            }
        }

        private void Stop(object arg)
        {
            View.Pause();
            DialogFacade.Confirm(Resources.PlayerResources.ConfirmStopVideo,
                (result) =>
                {
                    if (result)
                    {
                        Stop();
                    }
                    else
                    {
                        View.Play();
                    }
                });
        }

        private void Stop()
        {
            paused = false;
            StopTimers();
            Video.IsPlaying = false;
            View.Stop();
            View.Hide();
            IsMediaOpened = false;
        }

        private void StartTimers()
        {
            //telemetryTimer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1));
            //textTimer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private bool paused;
        private void Pause(object arg)
        {
            paused = true;
            StopTimers();
            Video.IsPlaying = false;
            View.Pause();
        }

        private void StopTimers()
        {
            //if(telemetryTimer != null)                
            //    telemetryTimer.Change(Timeout.Infinite, Timeout.Infinite);
            //if(textTimer != null)
            //    textTimer.Change(Timeout.Infinite, Timeout.Infinite);           
        }

        private void FullScreen(object arg)
        {
            IsFullScreen = !IsFullScreen;
        }

        public bool IsFullScreen
        {
            get { return Shell.IsFullScreen; }
            set
            {
                Shell.IsFullScreen = value;
                FirePropertyChanged("IsFullScreen");
            }
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
                LoadVideo(value);
                FirePropertyChanged("Video");
            }
        }

        private Queue<VideoText> textQueue = new Queue<VideoText>();

        private void LoadVideo(Video video)
        {
            foreach (var vt in video.VideoText.Where(x=>x.StartTime > video.PlayFrom).OrderBy(x => x.StartTime))
            {
                textQueue.Enqueue(vt);                
            }
            if (video.IsTelemetryLoaded)
            {
                LoadLinkedDictionary();
                View.LoadTelemetry(video.PlayingTelemetry);
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
            linked.Clear();
            foreach (var t in video.PlayingTelemetry)
            {
                if(!linked.ContainsKey(t.TimePosition.TotalSeconds))
                    linked.Add(t.TimePosition.TotalSeconds, t);
                queue.Enqueue(t);
            }            
        }

        void video_TelemetryLoaded(object sender, EventArgs e)
        {
            var video = sender as Video;
            LoadLinkedDictionary();
            View.LoadTelemetry(video.PlayingTelemetry);
        }

        public double ZeroSeconds
        {
            get { return 0; }
        }

        private TimeSpan playerPosition = new TimeSpan();
        public TimeSpan PlayerPosition
        {
            get { return playerPosition; }
            set
            {
                playerPosition = value;
                FirePropertyChanged("PlayerPosition");
                ThreadPool.QueueUserWorkItem(new WaitCallback(CheckForTextToDisplayAt));
                ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateTelemetryAt));
            }
        }

        private void CheckForTextToDisplayAt(object arg)
        {
            SmartDispatcher.BeginInvoke(() =>
                {
                    if (textQueue.Count > 0 && textQueue.Peek().StartTime <= PlayerPosition)
                    {
                        LoadVideoText(textQueue.Dequeue());
                        //SmartDispatcher.BeginInvoke(() => LoadVideoText(textQueue.Dequeue()));
                    }
                });
        }

        private void UpdateTelemetryAt(object arg)
        {
            SmartDispatcher.BeginInvoke(() =>
                {
                    if (queue.Peek().TimePosition <= PlayerPosition)
                        CurrentTelemetry = queue.Dequeue();
                });
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

        private Timer telemetryTimer;
        private Timer textTimer;

        public void MediaOpened()
        {
            //telemetryTimer = new Timer(new TimerCallback(obj =>
            //    {
            //        if (queue.Peek().TimePosition <= PlayerPosition)
            //            CurrentTelemetry = queue.Dequeue();
            //        System.GC.Collect();
            //    }), null, Timeout.Infinite, Timeout.Infinite);

            //textTimer = new Timer(new TimerCallback(obj =>
            //{
            //    if (textQueue.Count > 0 && textQueue.Peek().StartTime <= playerPosition)
            //    {
            //        SmartDispatcher.BeginInvoke
            //        (
            //            () => LoadVideoText(textQueue.Dequeue()
            //        ));
            //    }
            //    System.GC.Collect();
            //}), null, Timeout.Infinite, Timeout.Infinite);
            Video.IsMediaLoading = false;
            IsMediaOpened = true;
            View.SetStartPosition(Video.PlayFrom);
        }

        private void LoadVideoText(VideoText videoText)
        {
            videoText.IsShown = true;
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

        private bool mediaOpened;
        public bool IsMediaOpened
        {
            get { return mediaOpened; }
            set
            {
                mediaOpened = value;
                FirePropertyChanged("IsMediaOpened");
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
            Stop();

            //this.hasVideoEnded = true;
            //IsMediaOpened = false;
            //View.Cleanup();
            //View.Hide();
        }

        public void MediaError(SmoothStreamingErrorEventArgs e)
        {
            DialogFacade.Alert(string.Format(Resources.PlayerResources.MediaError, e.ErrorCode, e.ErrorMessage));
            Stop();
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

        private ICommand stopCommand;
        public ICommand StopCommand
        {
            get { return stopCommand; }
            set
            {
                stopCommand = value;
                FirePropertyChanged("StopCommand");
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

        public ICommand ExportTrainerFileCommand { get; set; }
    }
}
