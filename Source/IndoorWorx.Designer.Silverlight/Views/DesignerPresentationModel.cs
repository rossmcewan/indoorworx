﻿using System;
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
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure;
using Microsoft.Practices.Composite.Presentation.Commands;
using IndoorWorx.Infrastructure.Facades;
using IndoorWorx.Designer.Resources;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Designer.Events;
using IndoorWorx.Infrastructure.Helpers;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Services;
using IndoorWorx.Infrastructure.Events;

namespace IndoorWorx.Designer.Views
{
    public class DesignerPresentationModel : BaseModel, IDesignerPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IShell shell;
        private readonly IDialogFacade dialogFacade;
        private readonly IEventAggregator eventAggregator;

        public DesignerPresentationModel(IServiceLocator serviceLocator, IShell shell, IDialogFacade dialogFacade, IEventAggregator eventAggregator)
        {
            this.serviceLocator = serviceLocator;
            this.shell = shell;
            this.dialogFacade = dialogFacade;
            this.eventAggregator = eventAggregator;
            this.CancelCommand = new DelegateCommand<object>(Cancel);
            this.SaveCommand = new DelegateCommand<object>(Save);
            eventAggregator.GetEvent<IntervalSelectedEvent>().Subscribe(IntervalSelected, true);
        }

        private void IntervalSelected(Interval interval)
        {
            if (interval != null)
            {
                RangeFrom = interval.Position;
                RangeTo = RangeFrom.Add(interval.Duration);
            }
        }

        private DateTime rangeFrom;
        public virtual DateTime RangeFrom
        {
            get { return rangeFrom; }
            set
            {
                rangeFrom = value;
                FirePropertyChanged("RangeFrom");
            }
        }

        private DateTime rangeTo;
        public virtual DateTime RangeTo
        {
            get { return rangeTo; }
            set
            {
                rangeTo = value;
                FirePropertyChanged("RangeTo");
            }
        }

        public ICommand CancelCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        private void Cancel(object arg) 
        {
            if (SelectedTemplate != null)
            {
                foreach (var interval in SelectedTemplate.Intervals)
                {
                    interval.ClearDesignData();
                }
            }
            Hide();
        }

        private void Save(object arg) 
        {
            IsBusy = true;
            try
            {
                var service = serviceLocator.GetInstance<ITrainingSetService>();
                var trainingSet = new TrainingSet();
                trainingSet.TrainingSetTemplateId = SelectedTemplate.Id;
                trainingSet.Title = Title;
                if (UseSingleVideo)
                {
                    var part = new VideoPart();
                    part.VideoId = Video.Id;
                    part.From = VideoFrom;
                    part.To = VideoTo;
                    trainingSet.VideoParts.Add(part);
                }
                if (UseMultipleVideos)
                {
                    foreach (var set in SelectedTemplate.Sets)
                    {
                        if (set.UseSingleVideo)
                        {
                            var part = new VideoPart();
                            part.VideoId = set.Video.Id;
                            part.From = set.VideoFrom;
                            part.To = set.VideoTo;
                            trainingSet.VideoParts.Add(part);
                        }
                        if (set.UseMultipleVideos)
                        {
                            foreach (var interval in set.Intervals)
                            {
                                var part = new VideoPart();
                                part.VideoId = interval.Video.Id;
                                part.From = interval.VideoFrom;
                                part.To = interval.VideoTo;
                                trainingSet.VideoParts.Add(part);
                            }
                        }
                    }
                }
                service.CreateTrainingSetError += (sender, e) =>
                    {
                        IsBusy = false;
                        throw e.Value;
                    };
                service.TrainingSetCreated += (sender, e) =>
                    {
                        if (e.Value.Status == CreateTrainingSetStatus.Success)
                        {
                            ApplicationUser.CurrentUser.Videos.Add(e.Value.TrainingSet);
                            foreach (var set in SelectedTemplate.Sets)
                            {
                                set.ClearDesignData();
                            }
                            foreach (var interval in SelectedTemplate.Intervals)
                            {
                                interval.ClearDesignData();
                            }
                            IsBusy = false;
                            Hide();
                            eventAggregator.GetEvent<VideoCreatedEvent>().Publish(e.Value.TrainingSet);
                        }
                        else
                        {
                            dialogFacade.Alert(DesignerResources.ErrorCreatingWorkout);
                            IsBusy = false;
                        }                        
                    };
                service.CreateTrainingSet(trainingSet);
            }
            catch
            {
                IsBusy = false;
                throw;
            }
        }

        public IDesignerView View { get; set; }

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
                    RangeFrom = DateTimeHelper.ZeroTime;
                    RangeTo = RangeFrom.Add(selectedTemplate.Duration);
                }
                if (useMultipleVideos)
                {
                    if (SelectedInterval == null)
                        SelectedInterval = selectedTemplate.Sets.FirstOrDefault();
                    IntervalSelected(SelectedInterval);
                }
                FirePropertyChanged("SelectedInterval");
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
                    RangeFrom = DateTimeHelper.ZeroTime;
                    RangeTo = RangeFrom.Add(selectedTemplate.Duration);
                }
                if (useMultipleVideos)
                {
                    if (SelectedInterval == null)
                        SelectedInterval = selectedTemplate.Sets.FirstOrDefault();
                    else
                        IntervalSelected(SelectedInterval);
                }
                FirePropertyChanged("SelectedInterval");
                FirePropertyChanged("UseMultipleVideos");
                FirePropertyChanged("UseSingleVideo");
            }
        }

        private TrainingSetTemplate selectedTemplate;
        public virtual TrainingSetTemplate SelectedTemplate
        {
            get { return selectedTemplate; }
            set
            {
                selectedTemplate = value;
                if (selectedTemplate != null)
                {                    
                    selectedTemplate.ParseSets();
                    selectedTemplate.SetupIntervalTimes();
                    selectedTemplate.LoadTelemetry();
                    RangeFrom = DateTimeHelper.ZeroTime;
                    RangeTo = RangeFrom.Add(selectedTemplate.Duration);
                    Title = selectedTemplate.Title;
                }
                FirePropertyChanged("SelectedTemplate");
            }
        }

        private Interval selectedInterval;
        public virtual Interval SelectedInterval
        {
            get { return selectedInterval; }
            set
            {
                selectedInterval = value;
                IntervalSelected(selectedInterval);
                FirePropertyChanged("SelectedInterval");
                FirePropertyChanged("SelectableVideos");
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
                    if (length != selectedTemplate.Duration)
                    {
                        //video from has changed, attempt to move video to
                        videoTo = videoFrom.Add(selectedTemplate.Duration);
                        if (videoTo > video.Duration)
                        {
                            //it moved past the end, so send it back and adjust the video from accordingly
                            videoTo = video.Duration;
                            videoFrom = videoTo.Subtract(selectedTemplate.Duration);
                        }
                    }
                    videoTo = new TimeSpan(videoTo.Hours, videoTo.Minutes, videoTo.Seconds);
                    videoFrom = new TimeSpan(videoFrom.Hours, videoFrom.Minutes, videoFrom.Seconds);
                }
                if (SelectedInterval != null)
                {
                    SelectedInterval.VideoTo = videoTo;
                    SelectedInterval.VideoFrom = videoFrom;
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
                    if (length != selectedTemplate.Duration)
                    {
                        //video to has changed, attempt to move video from
                        videoFrom = videoTo.Subtract(selectedTemplate.Duration);
                        if (videoFrom < TimeSpan.Zero)
                        {
                            //it moved past the beginning, so set it to zero and adjust the video to accordingly
                            videoFrom = TimeSpan.Zero;
                            videoTo = videoFrom.Add(selectedTemplate.Duration);
                        }
                    }
                    videoTo = new TimeSpan(videoTo.Hours, videoTo.Minutes, videoTo.Seconds);
                    videoFrom = new TimeSpan(videoFrom.Hours, videoFrom.Minutes, videoFrom.Seconds);
                }
                if (SelectedInterval != null)
                {
                    SelectedInterval.VideoTo = videoTo;
                    SelectedInterval.VideoFrom = videoFrom;
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
                if (value != null && value.Duration < SelectedTemplate.Duration)
                {
                    dialogFacade.Alert(DesignerResources.SelectedVideoIsTooShortForTemplate);
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
                        VideoTo = VideoFrom.Add(selectedTemplate.Duration);
                    };
                    video.LoadTelemetry();
                }
                if(SelectedInterval != null)
                    SelectedInterval.Video = video;
                FirePropertyChanged("Video");
            }
        }

        public void Show()
        {
            shell.AddToLayoutRoot(View as UIElement);
        }

        public void Hide()
        {
            shell.RemoveFromLayoutRoot(View as UIElement);
        }

        private bool busy;
        public virtual bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                FirePropertyChanged("IsBusy");
            }
        }

        private string title;
        public virtual string Title
        {
            get { return title; }
            set
            {
                title = value;
                FirePropertyChanged("Title");
            }
        }

        public ICollection<Video> SelectableVideos
        {
            get
            {
                if (ApplicationUser.CurrentUser != null)
                {
                    var videos = ApplicationUser.CurrentUser.Videos;
                    if (SelectedTemplate != null)
                        return videos.Where(x => x.Catalog != null && x.Duration >= SelectedTemplate.Duration).ToList();
                    return videos;
                }
                return new List<Video>();
            }
        }
    }
}
