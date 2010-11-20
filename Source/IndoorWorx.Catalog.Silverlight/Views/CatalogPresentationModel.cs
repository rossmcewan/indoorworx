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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Services;
using IndoorWorx.Infrastructure;

namespace IndoorWorx.Catalog.Views
{
    public class CatalogPresentationModel : BaseModel, ICatalogPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        public CatalogPresentationModel(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        #region ICatalogPresentationModel Members

        private ICatalogView view;
        public ICatalogView View
        {
            get
            {
                return this.view;
            }
            set
            {
                this.view = value;
            }
        }

        private ICollection<Category> categories = new ObservableCollection<Category>();
        public System.Collections.Generic.ICollection<Infrastructure.Models.Category> Categories
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

        private Category selectedCategory;
        public Infrastructure.Models.Category SelectedCategory
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
                    SelectedCategory = Categories.FirstOrDefault();
                    if (SelectedCategory != null)
                    {
                        SelectedCategory.SelectedCatalog = SelectedCategory.Catalogs.FirstOrDefault();
                        if (SelectedCategory.SelectedCatalog != null)
                            SelectedCategory.SelectedCatalog.SelectedVideo = SelectedCategory.SelectedCatalog.Videos.FirstOrDefault();
                    }
                    this.IsBusy = false;
                };
            this.IsBusy = true;
            categoryService.RetrieveCategories();            
        }

        public void PlaySelectedPreview(Action play)
        {
            var video = SelectedCategory.SelectedCatalog.SelectedVideo ?? SelectedCategory.SelectedCatalog.Videos.FirstOrDefault();
            if (video != null)
            {
                SmartDispatcher.BeginInvoke(() => video.IsPlaying = true);
            }
            play();
        }

        public void StopSelectedPreview(Action stop)
        {
            var video = SelectedCategory.SelectedCatalog.SelectedVideo ?? SelectedCategory.SelectedCatalog.Videos.FirstOrDefault();
            if (video != null)
                SmartDispatcher.BeginInvoke(() => video.IsPlaying = false);
            stop();
        }

        private ICommand designTrainingSetCommand;
        public ICommand DesignTrainingSetCommand
        {
            get { return designTrainingSetCommand; }
            set
            {
                designTrainingSetCommand = value;
                FirePropertyChanged("DesignTrainingSetCommand");
            }
        }

        private ICommand playTrainingSetCommand;
        public ICommand PlayTrainingSetCommand
        {
            get { return playTrainingSetCommand; }
            set
            {
                playTrainingSetCommand = value;
                FirePropertyChanged("PlayTrainingSetCommand");
            }
        }

        #endregion
    }
}
