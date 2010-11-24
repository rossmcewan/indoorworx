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
using System.Threading;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    public partial class TrainingSet
    {
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

        [OnDeserialized]
        public new void OnDeserialized(StreamingContext context)
        {
            base.OnDeserialized(context);
            this.telemetry = new ObservableCollection<Telemetry>();
        }
    }
}
