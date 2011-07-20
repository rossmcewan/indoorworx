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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Services;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace IndoorWorx.Catalog.Views
{
    public class VideoCatalogPresentationModel : BaseModel, IVideoCatalogPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        public VideoCatalogPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            this.AddToMyLibraryCommand = new DelegateCommand<Video>(AddToMyLibrary);
        }

        public IVideoCatalogView View { get; set; }

        public ICommand AddToMyLibraryCommand { get; private set; }

        private void AddToMyLibrary(Video video)
        {
            ApplicationUser.CurrentUser.AddVideoToLibrary(video, () => { });
        }

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
                FirePropertyChanged("FilteredCategories");
                FirePropertyChanged("FilteredCatalogs");
                FirePropertyChanged("NumberOfVideosLabel");
            }
        }

        public ICollection<Category> FilteredCategories
        {
            get
            {
                if (string.IsNullOrWhiteSpace(filter) || filter == "ALL")
                    return Categories;
                return Categories.Where(x => x.Title == filter).ToList();
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

        public ICollection<IndoorWorx.Infrastructure.Models.Catalog> FilteredCatalogs
        {
            get
            {
                var catalogs = new List<IndoorWorx.Infrastructure.Models.Catalog>();
                foreach (var cat in FilteredCategories)
                {
                    catalogs.AddRange(cat.Catalogs);
                }
                return catalogs;
            }
        }

        public ICollection<Video> AllFilteredVideos
        {
            get
            {
                var result = new List<Video>();
                foreach (var cat in FilteredCategories)
                {
                    result.AddRange(cat.Videos);
                }
                return result;
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

        private string filter;
        public IVideoCatalogPresentationModel FilterVideosBy(string filter)
        {
            this.filter = filter;
            FirePropertyChanged("FilteredCategories");
            FirePropertyChanged("FilteredCatalogs");
            FirePropertyChanged("NumberOfVideosLabel");
            return this;
        }

        private string orderBy;
        public IVideoCatalogPresentationModel OrderVideosBy(string orderBy)
        {
            this.orderBy = orderBy;
            FirePropertyChanged("OrderBy");
            FirePropertyChanged("FilteredCategories");
            FirePropertyChanged("FilteredCatalogs");
            FirePropertyChanged("NumberOfVideosLabel");
            return this;
        }

        public string OrderBy
        {
            get { return this.orderBy; }
        }

        public string NumberOfVideosLabel
        {
            get { return string.Format(Resources.CatalogResources.NumberOfVideosLabel, AllFilteredVideos.Count); }
        }        
    }
}
