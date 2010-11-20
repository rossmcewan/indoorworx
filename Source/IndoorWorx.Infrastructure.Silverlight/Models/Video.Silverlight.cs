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
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class Video
    {
        public event EventHandler TelemetryLoaded;


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

        private Video selectedTrainingSet;
        public virtual Video SelectedTrainingSet
        {
            get { return selectedTrainingSet; }
            set
            {
                selectedTrainingSet = value;
                selectedTrainingSet.LoadTelemetry();
                FirePropertyChanged("SelectedTrainingSet");
            }
        }

        private bool telemetryActive;
        public virtual bool IsTelemetryActive
        {
            get { return telemetryActive; }
            set
            {
                telemetryActive = value;
                FirePropertyChanged("IsTelemetryActive");
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
            if (TelemetryUri == null)
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
                dataRetriever.DownloadStringAsync(TelemetryUri);
            }
        }

        private ICollection<Telemetry> telemetry;
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

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            this.IsMediaLoading = true;
        }
    }
}
