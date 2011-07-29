using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading;
using Microsoft.Practices.Composite.Events;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class Video
    {
        public Video()
        {
            Initialize();
        }

        private bool selected;
        public virtual bool IsSelected
        {
            get { return selected; }
            set
            {                
                selected = value;
                FirePropertyChanged("IsSelected");
            }
        }

        private bool playing = false;
        public virtual bool IsPlaying
        {
            get { return playing; }
            set
            {
                playing = value;
               
                FirePropertyChanged("IsPlaying");
            }
        }

        private bool mediaLoading;
        public virtual bool IsMediaLoading
        {
            get { return mediaLoading; }
            set
            {
                mediaLoading = value;
                FirePropertyChanged("IsMediaLoading");
            }
        }        

        public event EventHandler TelemetryLoaded;

        private bool telemetryLoading;
        public virtual bool IsTelemetryLoading
        {
            get { return telemetryLoading; }
            set
            {
                telemetryLoading = value;
                FirePropertyChanged("IsTelemetryLoading");
            }
        }

        private bool telemetryLoaded;
        public virtual bool IsTelemetryLoaded
        {
            get { return telemetryLoaded; }
            set
            {
                this.telemetryLoaded = value;
                FirePropertyChanged("IsTelemetryLoaded");
            }
        }

        public void LoadTelemetry()
        {
            if (TelemetryInfo.TelemetryUri == null)
                return;
            if (!IsTelemetryLoaded && !IsTelemetryLoading)
            {
                var _telemetry = new List<Telemetry>();
                IsTelemetryLoading = true;
                var dataRetriever = new System.Net.WebClient();
                dataRetriever.DownloadStringCompleted += (sender, e) =>
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
                    {
                        using (var reader = new StringReader(e.Result))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                try
                                {
                                    _telemetry.Add(Models.Telemetry.Parse(line));
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                        }
                        IsTelemetryLoading = false;
                        Telemetry = _telemetry;
                        IsTelemetryLoaded = true;
                        if (TelemetryLoaded != null)
                            TelemetryLoaded(this, EventArgs.Empty);
                    }));
                };
                dataRetriever.DownloadStringAsync(TelemetryInfo.TelemetryUri);
            }
        }

        private ICollection<Telemetry> telemetry = new ObservableCollection<Telemetry>();
        public virtual ICollection<Telemetry> Telemetry
        {
            get
            {
                return telemetry;
            }
            set
            {
                this.telemetry = value;
                FirePropertyChanged("Telemetry");
            }
        }

        public void LoadPlayingTelemetry()
        {
            this.PlayingTelemetry = Telemetry.Where(x => x.TimePosition > this.PlayFrom && x.TimePosition < this.PlayTo).ToList();
        }

        private ICollection<Telemetry> playingTelemetry;
        public virtual ICollection<Telemetry> PlayingTelemetry
        {
            get
            {
                return playingTelemetry;
            }
            set
            {
                this.playingTelemetry = value;
                FirePropertyChanged("PlayingTelemetry");
            }
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            Initialize();
            PlayTo = Duration;
        }

        private void Initialize()
        {
            this.IsMediaLoading = true;
            this.telemetry = new ObservableCollection<Telemetry>();
        }        

        private TimeSpan playFrom;
        public virtual TimeSpan PlayFrom
        {
            get { return playFrom; }
            set
            {
                playFrom = TimeSpan.FromSeconds(Math.Round(value.TotalSeconds, 0));
                FirePropertyChanged("PlayFrom");
                FirePropertyChanged("PlayDuration");
            }
        }

        private TimeSpan playTo;
        public virtual TimeSpan PlayTo
        {
            get { return playTo; }
            set
            {
                playTo = TimeSpan.FromSeconds(Math.Round(value.TotalSeconds, 0));
                FirePropertyChanged("PlayTo");
                FirePropertyChanged("PlayDuration");
            }
        }

        public TimeSpan PlayDuration
        {
            get { return PlayTo - PlayFrom; }
        }

        private DateTime reference;
        public DateTime StartDateTime
        {
            get { return reference; }
        }

        public DateTime EndDateTime
        {
            get { return reference.AddMilliseconds(Duration.TotalMilliseconds); }
        }
    }
}
