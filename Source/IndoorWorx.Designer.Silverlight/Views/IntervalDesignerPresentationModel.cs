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
using IndoorWorx.Infrastructure.Models;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Designer.Events;
using IndoorWorx.Designer.Resources;
using IndoorWorx.Infrastructure.Facades;
using Microsoft.Practices.Composite.Presentation.Events;
using System.Collections.Generic;

namespace IndoorWorx.Designer.Views
{
    public class IntervalDesignerPresentationModel : BaseModel, IIntervalDesignerPresentationModel
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogFacade dialogFacade;
        public IntervalDesignerPresentationModel(IEventAggregator eventAggregator, IDialogFacade dialogFacade)
        {
            this.eventAggregator = eventAggregator;
            this.dialogFacade = dialogFacade;
        }

        private bool allowSingleOrMultipleVideoSelection = true;
        public virtual bool AllowSingleOrMultipleVideoSelection
        {
            get { return allowSingleOrMultipleVideoSelection; }
            set
            {
                allowSingleOrMultipleVideoSelection = value;
                FirePropertyChanged("AllowSingleOrMultipleVideoSelection");
            }
        }

        public IIntervalDesignerView View { get; set; }

        private Interval interval;
        public virtual Interval Interval
        {
            get { return interval; }
            set
            {
                interval = value;
                if (interval is IntervalGroup)
                {
                    AllowSingleOrMultipleVideoSelection = (interval as IntervalGroup).Intervals.Count > 1;
                }
                if (interval != null)
                {
                    interval.UseMultipleVideos = this.UseMultipleVideos;
                    interval.UseSingleVideo = this.UseSingleVideo;
                }
                FirePropertyChanged("Interval");
            }
        }

        private Interval selectedInterval;
        public virtual Interval SelectedInterval
        {
            get { return selectedInterval; }
            set
            {
                selectedInterval = value;
                FirePropertyChanged("SelectedInterval");
                FirePropertyChanged("SelectableVideos");
                eventAggregator.GetEvent<IntervalSelectedEvent>().Publish(selectedInterval);
            }
        }

        private bool useSingleVideo = true;
        public virtual bool UseSingleVideo
        {
            get { return useSingleVideo; }
            set
            {
                useSingleVideo = value;
                useMultipleVideos = !useSingleVideo;
                if (useSingleVideo)
                {
                    eventAggregator.GetEvent<IntervalSelectedEvent>().Publish(Interval);
                }
                if (useMultipleVideos)
                {
                    if(SelectedInterval == null && interval is IntervalGroup)
                        SelectedInterval = (interval as IntervalGroup).Intervals.FirstOrDefault();
                    else
                        eventAggregator.GetEvent<IntervalSelectedEvent>().Publish(SelectedInterval);
                }
                if (Interval != null)
                {
                    Interval.UseSingleVideo = useSingleVideo;
                    Interval.UseMultipleVideos = useMultipleVideos;
                }
                FirePropertyChanged("UseSingleVideo");
                FirePropertyChanged("UseMultipleVideos");
            }
        }

        private bool useMultipleVideos = false;
        public virtual bool UseMultipleVideos
        {
            get { return useMultipleVideos; }
            set
            {
                useMultipleVideos = value;
                useSingleVideo = !useMultipleVideos;
                if (useSingleVideo)
                {
                    eventAggregator.GetEvent<IntervalSelectedEvent>().Publish(Interval);
                }
                if (useMultipleVideos)
                {
                    if (SelectedInterval == null && interval is IntervalGroup)
                        SelectedInterval = (interval as IntervalGroup).Intervals.FirstOrDefault();
                    else
                        eventAggregator.GetEvent<IntervalSelectedEvent>().Publish(SelectedInterval);
                }
                if (Interval != null)
                {
                    Interval.UseSingleVideo = useSingleVideo;
                    Interval.UseMultipleVideos = useMultipleVideos;
                }
                FirePropertyChanged("UseMultipleVideos");
                FirePropertyChanged("UseSingleVideo");
            }
        }

        private TimeSpan videoFrom;
        public virtual TimeSpan VideoFrom
        {
            get { return videoFrom; }
            set
            {
                videoFrom = value;
                if (video != null)
                {
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
                    videoTo = new TimeSpan(videoTo.Hours, videoTo.Minutes, videoTo.Seconds);
                    videoFrom = new TimeSpan(videoFrom.Hours, videoFrom.Minutes, videoFrom.Seconds);
                }
                if (Interval != null)
                {
                    Interval.VideoTo = videoTo;
                    Interval.VideoFrom = videoFrom;
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
                videoTo = value;
                if (video != null)
                {
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
                    videoTo = new TimeSpan(videoTo.Hours, videoTo.Minutes, videoTo.Seconds);
                    videoFrom = new TimeSpan(videoFrom.Hours, videoFrom.Minutes, videoFrom.Seconds);
                }
                if (Interval != null)
                {
                    Interval.VideoTo = videoTo;
                    Interval.VideoFrom = videoFrom;
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
                if (value != null && Interval != null && value.Duration < Interval.Duration)
                {
                    if(Interval is IntervalGroup)
                        dialogFacade.Alert(DesignerResources.SelectedVideoIsTooShortForIntervalGroup);
                    else
                        dialogFacade.Alert(DesignerResources.SelectedVideoIsTooShortForInterval);
                    FirePropertyChanged("Video");
                    return;
                }
                video = value;
                if (video != null)
                    video.IsMediaLoading = true;
                if (video != null)
                {
                    video.TelemetryLoaded += (sender, e) =>
                    {
                        VideoTo = VideoFrom.Add(Interval.Duration);
                    };                    
                    video.LoadTelemetry();
                }
                if (Interval != null)
                {
                    Interval.Video = video;
                }
                FirePropertyChanged("Video");
            }
        }

        public ICollection<Video> SelectableVideos
        {
            get
            {
                var videos = ApplicationUser.CurrentUser.Videos;
                return videos.Where(x => x.Catalog != null && (Interval != null && x.Duration >= Interval.Duration)).ToList();
            }
        }
    }
}
