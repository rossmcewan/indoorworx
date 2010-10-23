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
using System.ComponentModel;
using VideoPlayerTelemetry.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Windows.Threading;
using VideoPlayerTelemetry.DataServiceReference;
using VideoPlayerTelemetry.Views;

namespace VideoPlayerTelemetry.ViewModels
{
    public class PlayerViewModel : BaseModel
    {
        DispatcherTimer timer = new DispatcherTimer();

        private IPlayer player;

        public PlayerViewModel(IPlayer player)
        {
            this.player = player;
            this.Video = new Video();
            InitializeData();
        }

        public static TimeSpan _CurrentPlaybackPosition;


        public TimeSpan CurrentPlaybackPosition
        {
            get { return _CurrentPlaybackPosition; }
            set
            {
                _CurrentPlaybackPosition = value;
                OnPropertyChanged("CurrentPlaybackPosition");
            }
        }

        private int lastItemIndex;

        DateTime dt = DateTime.Now;

        public DateTime CurrentDateTimePlaybackPosition
        {
            get
            {
               return new DateTime(dt.Year, dt.Month, dt.Day, CurrentPlaybackPosition.Hours, CurrentPlaybackPosition.Minutes, CurrentPlaybackPosition.Seconds);
            }
        }

        private void IncrementPosition()
        {
            CurrentPlaybackPosition = player.GetPlayPosition();
            var index = Convert.ToInt32(player.GetPlayPosition().TotalSeconds / 2 - 1);            
            var rootTelemetry = Data.ElementAtOrDefault(index);
            if (rootTelemetry != null && !rootTelemetry.IsAdded)
            {
                CurrentTelemetry = rootTelemetry;
                TrackingData.Add(rootTelemetry);
                rootTelemetry.IsAdded = true;

                var telem = zoomedData.First();
                lastItemIndex++;
                zoomedData.Remove(telem);
                var lastItem = data.ElementAt(lastItemIndex);
                zoomedData.Add(lastItem);
                var dt = DateTime.Now;
                var zd = lastItem.TimePosition;
                ZoomedStartTime = new DateTime(dt.Year, dt.Month, dt.Day, CurrentPlaybackPosition.Hours, CurrentPlaybackPosition.Minutes, CurrentPlaybackPosition.Seconds);
                ZoomedTotalTime = new DateTime(dt.Year, dt.Month, dt.Day, zd.Hours, zd.Minutes, zd.Seconds);
                ZoomedMaxPercentage = this.zoomedData.Max(x => x.PercentageOfThreshold) + 10M;

                if (CurrentInformation.EndTime >= CurrentPlaybackPosition)
                {
                    CurrentInformation.Clear();
                }
                if (information.Peek().StartTime <= CurrentPlaybackPosition)
                {
                    CurrentInformation = Information.Dequeue();
                }


                //ZoomedStartTime = CurrentDateTimePlaybackPosition.Subtract(TimeSpan.FromMinutes(2));
                //ZoomedTotalTime = CurrentDateTimePlaybackPosition.Add(TimeSpan.FromMinutes(6));

                //ZoomedStartTime = CurrentDateTimePlaybackPosition;
                //ZoomedTotalTime = CurrentDateTimePlaybackPosition.Add(TimeSpan.FromMinutes(8));
            }            
        }

        private Telemetry currentTelemetry;

        public Telemetry CurrentTelemetry
        {
            get { return currentTelemetry; }
            set 
            { 
                currentTelemetry = value;
                OnPropertyChanged("CurrentTelemetry");
            }
        }


        public int VerticalLineHeight
        {
            get { return 150; }
        }


        private void InitializeData()
        {
            this.Position = new Point(0, 70);
            DataServiceClient client = new DataServiceClient();
            client.ProvideTelemetryDataCompleted += new EventHandler<ProvideTelemetryDataCompletedEventArgs>(client_ProvideTelemetryDataCompleted);
            client.ProvideTelemetryDataAsync();
        }


        void client_ProvideTelemetryDataCompleted(object sender, ProvideTelemetryDataCompletedEventArgs e)
        {
            this.Data = Smooth(e.Result);
            var dt = DateTime.Now;
            var ts = this.data.Last().TimePosition;
            TotalTime = new DateTime(dt.Year, dt.Month, dt.Day, ts.Hours, ts.Minutes, ts.Seconds);
            MaxPercentage = this.data.Max(x => x.PercentageOfThreshold) + 10M;

            ZoomedData = new ObservableCollection<Telemetry>(Data.TakeWhile(d => d.TimePosition.TotalMinutes < 5).ToList());
            var lastItem = this.zoomedData.Last();
            lastItemIndex = this.zoomedData.Count - 1;// data.ToList().IndexOf(lastItem);

            var zd = lastItem.TimePosition; // Data.Where(d => d.TimePosition.TotalMinutes >= 8).First().TimePosition;
            ZoomedTotalTime = new DateTime(dt.Year, dt.Month, dt.Day, zd.Hours, zd.Minutes, zd.Seconds);
            ZoomedMaxPercentage = this.zoomedData.Max(x => x.PercentageOfThreshold) + 10M;

            LoadInformation();

            //ZoomedStartTime = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0,0);
            //ZoomedTotalTime = new DateTime(dt.Year, dt.Month, dt.Day, 0, 8, 0);// zd.Hours, zd.Minutes, zd.Seconds);
            //ZoomedMaxPercentage = Data.Max(x => x.PercentageOfThreshold) + 10M;


            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (_sender, _e) =>
            {
                IncrementPosition();
            };
            timer.Start();
            player.Play();
        }

        private void LoadInformation()
        {
            information = new Queue<Information>();
            currentInformation = new Information();
            Information.Enqueue(new Information()
            {
                Text = "Welcome to this V02 Max workout. It's gonna be tough, so let's get going with a nice warmup. Brace yourselves!!",
                StartTime = new TimeSpan(0,0,0),
                EndTime = new TimeSpan(0,0,30)
            });

            Information.Enqueue(new Information()
            {
                Text = "Its getting hot in here!!!",
                StartTime = new TimeSpan(0, 1, 0),
                EndTime = new TimeSpan(0, 1, 10)
            });

            Information.Enqueue(new Information()
            {
                Text = "We are about to get started with our first set. 2x2min (2min RI) + 1x 5min (5min RI).",
                StartTime = new TimeSpan(0, 26, 20),
                EndTime = new TimeSpan(0, 26, 35)
            });

            Information.Enqueue(new Information()
            {
                Text = "Lets Go!",
                StartTime = new TimeSpan(0, 26, 45),
                EndTime = new TimeSpan(0, 25, 55)
            });

            Information.Enqueue(new Information()
            {
                Text = "Go!",
                StartTime = new TimeSpan(0, 30, 45),
                EndTime = new TimeSpan(0, 30, 55)
            });
        }

        private ICollection<Telemetry> Smooth(ObservableCollection<Telemetry> rootTelemetry)
        {
            int counter = 0;
            while (counter <= rootTelemetry.Count)
            {
                var taken = rootTelemetry.Skip(counter).Take(15);
                foreach (var t in taken)
                    t.PercentageOfThreshold = taken.Average(x => x.PercentageOfThreshold);
                counter += 15;
            }
            return rootTelemetry;
        }

        private Information currentInformation;
        public Information CurrentInformation
        {
            get { return currentInformation; }
            set
            {
                currentInformation = value;
                OnPropertyChanged("CurrentInformation");
            }
        }

        private Queue<Information> information;
        public Queue<Information> Information
        {
            get { return information; }
            set
            {
                information = value;
                OnPropertyChanged("Information");
            }
        }

        private DateTime totalTime = DateTime.Today;
        public DateTime TotalTime
        {
            get { return this.totalTime; }
            set
            {
                this.totalTime = value;
                OnPropertyChanged("TotalTime");
            }
        }

        private DateTime zoomedTotalTime = DateTime.Today;
        public DateTime ZoomedTotalTime
        {
            get { return this.zoomedTotalTime; }
            set
            {
                this.zoomedTotalTime = value;
                OnPropertyChanged("ZoomedTotalTime");
            }
        }

        private DateTime zoomedStartTime = DateTime.Today;
        public DateTime ZoomedStartTime
        {
            get { return this.zoomedStartTime; }
            set
            {
                this.zoomedStartTime = value;
                OnPropertyChanged("ZoomedStartTime");
            }
        }

        private double zoomedInterval = 1;
        public double ZoomedInterval
        {
            get { return this.zoomedInterval; }
            set
            {
                this.zoomedInterval = value;
                OnPropertyChanged("ZoomedInterval");
            }
        }


        private DateTime min = DateTime.Today;
        public DateTime Minimum
        {
            get { return min; }
        }

        private double interval = 5;
        public double Interval
        {
            get { return this.interval; }
            set
            {
                this.interval = value;
                OnPropertyChanged("Interval");
            }
        }

        private TimeSpan playbackPosition = TimeSpan.Zero;

        public TimeSpan PlaybackPosition
        {
            get { return playbackPosition; }
            set 
            { 
                playbackPosition = value;
                OnPropertyChanged("PlaybackPosition");
            }
        }

        private Zones zones = new Zones();
        public Zones Zones
        {
            get { return zones; }
            set { zones = value; }
        }

        private Point position;
        public Point Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                OnPropertyChanged("Position");
            }
        }

        private ChartingData knob360Data;
        public ChartingData Knob360Data
        {
            get { return this.knob360Data; }
            set
            {
                this.knob360Data = value;
                OnPropertyChanged("Knob360Data");
            }
        }

        private Video video = null;
        public Video Video
        {
            get { return video; }
            set
            {
                video = value; OnPropertyChanged("Video");
            }
        }

        private ICollection<Telemetry> data = new ObservableCollection<Telemetry>();
        public ICollection<Telemetry> Data
        {
            get { return data; }
            set
            {
                data = value; 
                OnPropertyChanged("Data");
            }
        }

        private ICollection<Telemetry> zoomedData = new ObservableCollection<Telemetry>();
        public ICollection<Telemetry> ZoomedData
        {
            get { return zoomedData; }
            set
            {
                zoomedData = value;
                OnPropertyChanged("ZoomedData");
            }
        }

        private ICollection<Telemetry> trackingData = new ObservableCollection<Telemetry>();
        public ICollection<Telemetry> TrackingData
        {
            get { return trackingData; }
            set
            {
                trackingData = value;
                OnPropertyChanged("TrackingData");
            }
        }

        private decimal maxPercentage;
        public decimal MaxPercentage
        {
            get { return this.maxPercentage; }
            set
            {
                this.maxPercentage = value;
                OnPropertyChanged("MaxPercentage");
            }
        }

        private decimal zoomedMaxPercentage;
        public decimal ZoomedMaxPercentage
        {
            get { return this.zoomedMaxPercentage; }
            set
            {
                this.zoomedMaxPercentage = value;
                OnPropertyChanged("ZoomedMaxPercentage");
            }
        }
    }
}
