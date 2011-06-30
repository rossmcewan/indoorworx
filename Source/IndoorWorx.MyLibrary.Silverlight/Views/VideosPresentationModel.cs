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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Services;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.DragDrop;
using IndoorWorx.Library.DragDrop;
using IndoorWorx.Infrastructure;
using IndoorWorx.MyLibrary.Resources;
using Microsoft.Practices.Composite.Presentation.Commands;
using IndoorWorx.Infrastructure.Events;

namespace IndoorWorx.MyLibrary.Views
{
    public class VideosPresentationModel : BaseModel, IVideosPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        
        public VideosPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            ApplicationContext.Current.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "VideoCount")
                        FirePropertyChanged("NumberOfVideosLabel");
                };
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            this.playVideoCommand = new DelegateCommand<Video>(PlayVideo);
        }

        public void PlayVideo(Video video)
        {
            eventAggregator.GetEvent<PlayVideoEvent>().Publish(video);
        }

        private ICommand playVideoCommand;
        public ICommand PlayVideoCommand
        {
            get { return this.playVideoCommand; }
        }

        public IVideosView View { get; set; }

        public void Refresh()
        {            
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

        public string NumberOfVideosLabel
        {
            get { return string.Format(MyLibraryResources.NumberOfVideosLabel, ApplicationContext.Current.VideoCount); }
        }
    }
}
