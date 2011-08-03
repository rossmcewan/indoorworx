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
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Designer.Views
{
    public class IntervalDesignerPresentationModel : BaseModel, IIntervalDesignerPresentationModel
    {
        public IIntervalDesignerView View { get; set; }

        private Interval interval;
        public virtual Interval Interval
        {
            get { return interval; }
            set
            {
                interval = value;
                FirePropertyChanged("Interval");
            }
        }

        private bool useSingleVideo = false;
        public virtual bool UseSingleVideo
        {
            get { return useSingleVideo; }
            set
            {
                useSingleVideo = value;
                useMultipleVideos = !useSingleVideo;
                FirePropertyChanged("UseSingleVideo");
                FirePropertyChanged("UseMultipleVideos");
            }
        }

        private bool useMultipleVideos = true;
        public virtual bool UseMultipleVideos
        {
            get { return useMultipleVideos; }
            set
            {
                useMultipleVideos = value;
                useSingleVideo = !useMultipleVideos;
                FirePropertyChanged("UseMultipleVideos");
                FirePropertyChanged("UseSingleVideo");
            }
        }

        public virtual TimeSpan MinRange
        {
            get { return this.Interval.Duration.Subtract(TimeSpan.FromMilliseconds(1)); }
        }

        public virtual TimeSpan MaxRange
        {
            get { return this.Interval.Duration; }
        }

        private TimeSpan videoFrom;
        public virtual TimeSpan VideoFrom
        {
            get { return videoFrom; }
            set
            {
                //var length = videoTo.Subtract(value);
                //if (length == interval.Duration)
                //{
                //    videoFrom = value;
                //}
                //FirePropertyChanged("VideoFrom");
                videoFrom = value;
                var length = videoTo.Subtract(videoFrom);
                if (length != interval.Duration)
                {
                    //video from has changed, attempt to move video to
                    videoTo = videoFrom.Add(interval.Duration);
                    if (videoTo > video.Duration)
                    {
                        //it moved past the end, so send it back and adjust the video from accordingly
                        videoTo = video.Duration;
                        videoFrom = videoTo.Subtract(interval.Duration);
                    }
                }
                FirePropertyChanged("VideoFrom");
                FirePropertyChanged("VideoTo");
            }
        }

        private TimeSpan videoTo;
        public virtual TimeSpan VideoTo
        {
            get { return videoTo; }
            set
            {
                //var length = value.Subtract(videoFrom);
                //if (length == interval.Duration)
                //{
                //    videoTo = value;
                //}
                //FirePropertyChanged("VideoTo");
                videoTo = value;
                var length = videoTo.Subtract(videoFrom);
                if (length != interval.Duration)
                {
                    //video to has changed, attempt to move video from
                    videoFrom = videoTo.Subtract(interval.Duration);
                    if (videoFrom < TimeSpan.Zero)
                    {
                        //it moved past the beginning, so set it to zero and adjust the video to accordingly
                        videoFrom = TimeSpan.Zero;
                        videoTo = videoFrom.Add(interval.Duration);
                    }
                }
                FirePropertyChanged("VideoTo");
                FirePropertyChanged("VideoFrom");
            }
        }

        private Video video;
        public virtual Video Video
        {
            get { return video; }
            set
            {
                video = value;
                if (video != null)
                {
                    video.TelemetryLoaded += (sender, e) =>
                    {
                        VideoTo = VideoFrom.Add(Interval.Duration);
                    };
                    video.LoadTelemetry();
                }
                FirePropertyChanged("Video");
                FirePropertyChanged("MaxRange");
                FirePropertyChanged("MinRange");
            }
        }   
    }
}
