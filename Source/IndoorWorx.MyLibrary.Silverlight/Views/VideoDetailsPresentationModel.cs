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
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Services;
using Microsoft.Practices.Composite.Presentation.Commands;
using IndoorWorx.Infrastructure.Events;

namespace IndoorWorx.MyLibrary.Views
{
    public class VideoDetailsPresentationModel : BaseModel, IVideoDetailsPresentationModel
    {
        private IEventAggregator eventAggregator;
        private IServiceLocator serviceLocator;
        public VideoDetailsPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            this.PreviewVideoCommand = new DelegateCommand<Video>(PreviewVideo);
        }

        public void PreviewVideo(Video video)
        {
            eventAggregator.GetEvent<PreviewVideoEvent>().Publish(video);
        }

        public IVideoDetailsView View { get; set; }

        private Video video;
        public virtual Video Video
        {
            get { return video; }
            set
            {
                video = value;
                FirePropertyChanged("Video");
            }
        }

        public void SelectVideoWithId(Guid id)
        {
            var categoryService = serviceLocator.GetInstance<ICategoryService>();
            categoryService.CategoriesRetrieved += (sender, e) =>
                {
                    var categories = e.Value;
                    foreach (var category in categories)
                    {
                        var video = category.Videos.FirstOrDefault(x => x.Id == id);
                        if (video != null)
                        {
                            Video = video;
                            Video.LoadTelemetry();
                            break;
                        }
                    }
                    IsBusy = false;
                };
            categoryService.CategoryRetrievalError += (sender,e)=>
                {
                    IsBusy = false;
                };
            IsBusy = true;
            categoryService.RetrieveCategories();
        }

        public ICommand PreviewVideoCommand { get; private set; }
               
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
    }
}
