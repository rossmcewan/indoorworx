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
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Practices.Composite.Presentation.Commands;
using IndoorWorx.Infrastructure.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IndoorWorx.Infrastructure;
using Microsoft.Practices.Composite.Events;

namespace IndoorWorx.Designer.Views
{
    public class DesignerPresentationModel : BaseModel, IDesignerPresentationModel
    {
        public event EventHandler<DataEventArgs<Video>> VideoSelected;

        private IServiceLocator serviceLocator;
        public DesignerPresentationModel(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            this.AddDesignerCommand = new DelegateCommand<IDesignerPresentationModel>(AddDesigner);
        }

        public IDesignerView View { get; set; }

        private ICommand addDesignerCommand;
        public ICommand AddDesignerCommand
        {
            get { return addDesignerCommand; }
            set
            {
                addDesignerCommand = value;
                FirePropertyChanged("AddDesignerCommand");
            }
        }

        private void AddDesigner(IDesignerPresentationModel model)
        {
            View.AddDesigner();
        }

        private bool busy;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                FirePropertyChanged("IsBusy");
            }
        }

        private ICollection<Category> categories = new ObservableCollection<Category>();
        public ICollection<Category> Categories
        {
            get
            {
                return this.categories;
            }
            set
            {
                this.categories.Clear();
                foreach (var category in value)
                    this.categories.Add(category);
                FirePropertyChanged("Categories");
            }
        }

        private Video selectedVideo;
        public Video SelectedVideo
        {
            get { return selectedVideo; }
            set
            {
                selectedVideo = value;
                FirePropertyChanged("SelectedVideo");
                if (VideoSelected != null)
                    VideoSelected(this, new DataEventArgs<Video>(value));
                if (value != null)
                    value.LoadTelemetry();                
            }
        }

        public void LoadCategories()
        {
            var categoryService = serviceLocator.GetInstance<ICategoryService>();
            categoryService.CategoryRetrievalError += (sender, e) =>
            {
                this.IsBusy = false;
                throw e.Value;
            };
            categoryService.CategoriesRetrieved += (sender, e) =>
            {
                Categories = e.Value;
                this.IsBusy = false;
            };
            this.IsBusy = true;
            categoryService.RetrieveCategories();
        }

        public void PlaySelectedPreview(Action play)
        {
            var video = SelectedVideo;
            if (video != null)
            {
                SmartDispatcher.BeginInvoke(() => video.IsPlaying = true);
            }
            play();
        }

        public void StopSelectedPreview(Action stop)
        {
            var video = SelectedVideo;
            if (video != null)
                SmartDispatcher.BeginInvoke(() => video.IsPlaying = false);
            stop();
        }
    }
}
