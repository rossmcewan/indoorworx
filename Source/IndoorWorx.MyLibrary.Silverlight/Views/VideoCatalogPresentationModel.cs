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
using IndoorWorx.Infrastructure.Services;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.MyLibrary.Resources;
using IndoorWorx.Infrastructure;

namespace IndoorWorx.MyLibrary.Views
{
    public class VideoCatalogPresentationModel : BaseModel, IVideoCatalogPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        public VideoCatalogPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            ApplicationContext.Current.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "VideoCount")
                    {
                        FirePropertyChanged("NumberOfVideosLabel");
                        LoadCategories();
                    }
                };
        }

        public IVideoCatalogView View { get; set; }

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
                foreach (var video in ApplicationUser.CurrentUser.Videos)
                {                    
                    foreach (var cat in FilteredCategories)
                    {
                        if ((video.Catalog == null && FilteredCategories.Any(x => x.Title.ToLower() == "workouts")) || (video.Catalog != null && video.Catalog.Category.Equals(cat)))
                        {
                            if(!result.Contains(video))
                                result.Add(video);
                        }
                        //result.AddRange(cat.Videos);
                    }
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
                Categories = LoadCategoriesWithUsersVideos(e.Value);
                SelectedCategory = Categories.FirstOrDefault();
                this.IsBusy = false;
            };
            this.IsBusy = true;
            categoryService.RetrieveCategories();
        }

        private ICollection<Category> LoadCategoriesWithUsersVideos(ICollection<Category> sourceCategories)
        {
            var loadedCategories = new List<Category>();
            var _workoutCategory = sourceCategories.Last();
            var workoutCatalog = new Catalog() { Videos = new List<Video>() };
            var workoutCategory = new Category()
            {
                Id = _workoutCategory.Id,
                Description = _workoutCategory.Description,
                Title = _workoutCategory.Title,
                LibraryUri = _workoutCategory.LibraryUri,
                Catalogs = new List<Catalog>() { workoutCatalog }
            };
            foreach (var sourceCategory in sourceCategories)
            {
                var cat = new Category()
                {
                    Id = sourceCategory.Id,
                    Description = sourceCategory.Description,
                    Title = sourceCategory.Title,
                    LibraryUri = sourceCategory.LibraryUri,
                    Catalogs = new List<Catalog>()
                };
                bool hasContent = false;
                foreach (var catalog in sourceCategory.Catalogs)
                {
                    hasContent = true;
                    var _catalog = new Catalog()
                    {
                        Id = catalog.Id,
                        Description = catalog.Description,
                        ImageUri = catalog.ImageUri,
                        Title = catalog.Title,
                        Videos = new List<Video>()
                    };
                    if (ApplicationUser.CurrentUser != null)
                    {
                        foreach (var video in ApplicationUser.CurrentUser.Videos)
                        {
                            if (video.Catalog == null)
                            {
                                if (!workoutCatalog.Videos.Contains(video))
                                    workoutCatalog.Videos.Add(video);
                            }
                            else if (video.Catalog.Equals(catalog))
                            {
                                _catalog.Videos.Add(video);
                            }
                        }
                    }
                    cat.Catalogs.Add(_catalog);
                }
                if(hasContent)
                    loadedCategories.Add(cat);
            }
            loadedCategories.Add(workoutCategory);
            return loadedCategories;
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
            get { return string.Format(MyLibraryResources.NumberOfVideosLabel, AllFilteredVideos.Count); }
        }
    }
}
