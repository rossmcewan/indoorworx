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
        public event EventHandler SelectedTrainingSetChanging;
        public event EventHandler SelectedTrainingSetChanged;

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

        private TrainingSet selectedTrainingSet;
        public virtual TrainingSet SelectedTrainingSet
        {
            get { return selectedTrainingSet; }
            set
            {
                OnSelectedTrainingSetChanging();
                bool changed = value != selectedTrainingSet;
                selectedTrainingSet = value;
                if (selectedTrainingSet != null)
                    selectedTrainingSet.LoadTelemetry();
                if (changed)
                    OnSelectedTrainingSetChanged();
                FirePropertyChanged("SelectedTrainingSet");
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
            this.PlayingTelemetry = Telemetry.Where(x => x.TimePosition.TotalSeconds > this.PlayFrom && x.TimePosition.TotalSeconds < this.PlayTo).ToList();
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
            PlayTo = Duration.TotalSeconds;
        }

        private void Initialize()
        {
            this.IsMediaLoading = true;
            this.telemetry = new ObservableCollection<Telemetry>();
        }

        protected virtual void OnSelectedTrainingSetChanging()
        {
            if (SelectedTrainingSetChanging != null)
                SelectedTrainingSetChanging(this, EventArgs.Empty);
        }

        protected virtual void OnSelectedTrainingSetChanged()
        {
            if (SelectedTrainingSetChanged != null)
                SelectedTrainingSetChanged(this, EventArgs.Empty);
        }

        private double playFrom;
        public virtual double PlayFrom
        {
            get { return playFrom; }
            set
            {
                playFrom = value;
                FirePropertyChanged("PlayFrom");
                FirePropertyChanged("PlayDuration");
            }
        }

        private double playTo;
        public virtual double PlayTo
        {
            get { return playTo; }
            set
            {
                playTo = value;
                FirePropertyChanged("PlayTo");
                FirePropertyChanged("PlayDuration");
            }
        }

        public double PlayDuration
        {
            get { return PlayTo - PlayFrom; }
        }
    }
}
