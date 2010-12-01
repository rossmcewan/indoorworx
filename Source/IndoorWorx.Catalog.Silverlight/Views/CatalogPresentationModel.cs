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
using Microsoft.Practices.Composite.Presentation.Commands;
using IndoorWorx.Infrastructure.Navigation;
using Telerik.Windows.Controls;
using Microsoft.Practices.Composite.Events;
using IndoorWorx.Infrastructure.Events;

namespace IndoorWorx.Catalog.Views
{
    public class CatalogPresentationModel : BaseModel, ICatalogPresentationModel
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        public CatalogPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            this.DesignTrainingSetCommand = new DelegateCommand<Video>(DesignTrainingSet);
            this.PlayTrainingSetCommand = new DelegateCommand<Video>(PlayTrainingSet);
            this.PreviewTrainingSetCommand = new DelegateCommand<Video>(PreviewTrainingSet);
        }

        private IShell Shell
        {
            get { return this.serviceLocator.GetInstance<IShell>(); }
        }

        private void DesignTrainingSet(Video video)
        {
            eventAggregator.GetEvent<DesignVideoEvent>().Publish(video);            
        }

        private void PlayTrainingSet(Video video)
        {            
            eventAggregator.GetEvent<PlayVideoEvent>().Publish(video);            
        }

        private void PreviewTrainingSet(Video video)
        {
            eventAggregator.GetEvent<PreviewVideoEvent>().Publish(video);
        }

        #region ICatalogPresentationModel Members

        private object selectedItem;
        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                if (selectedItem is Category)
                    SelectedCategory = value as Category;
                if (selectedItem is IndoorWorx.Infrastructure.Models.Catalog)
                {
                    var catalog = selectedItem as IndoorWorx.Infrastructure.Models.Catalog;
                    foreach (var category in Categories)
                    {
                        foreach (var c in category.Catalogs)
                        {
                            if (c.Id == catalog.Id)
                            {
                                SelectedCategory = category;
                                category.SelectedCatalog = c;
                                break;
                            }
                        }
                    }
                }
                if (selectedItem is Video)
                {
                    var video = selectedItem as Video;
                    foreach (var category in Categories)
                    {
                        foreach (var c in category.Catalogs)
                        {
                            foreach (var v in c.Videos)
                            {
                                if (v.Id == video.Id)
                                {
                                    SelectedCategory = category;
                                    category.SelectedCatalog = c;
                                    c.SelectedVideo = v;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

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
                    this.IsBusy = false;
                };
            this.IsBusy = true;
            categoryService.RetrieveCategories();            
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

        private ICommand previewTrainingSetCommand;
        public ICommand PreviewTrainingSetCommand
        {
            get { return previewTrainingSetCommand; }
            set
            {
                previewTrainingSetCommand = value;
                FirePropertyChanged("PreviewTrainingSetCommand");
            }
        }

        #endregion
    }
}
