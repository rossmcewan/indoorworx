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
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;                        
        }

        public IVideosView View { get; set; }

        public void Refresh()
        {
            LoadCategories();
        }

        private ICollection<Category> categories;
        public ICollection<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                FirePropertyChanged("Categories");
            }
        }

        private Category selectedCategory;
        public Category SelectedCategory
        {
            get
            {
                return this.selectedCategory;
            }
            set
            {
                this.selectedCategory = value;
                FirePropertyChanged("SelectedCategory");
            }
        }

        private void LoadCategories()
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
                SelectedCategory = Categories.FirstOrDefault();
                this.IsBusy = false;
            };
            this.IsBusy = true;
            categoryService.RetrieveCategories();
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

        private ICommand playVideoCommand;
        public ICommand PlayVideoCommand
        {
            get { return this.playVideoCommand; }
        }

        public string NumberOfVideosLabel
        {
            get { return string.Format(MyLibraryResources.NumberOfVideosLabel, ApplicationContext.Current.VideoCount); }
        }
    }
}
