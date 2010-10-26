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
using IndoorWorx.Catalog.Services;

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

        public void LoadCategories()
        {
            var categoryService = serviceLocator.GetInstance<ICategoryService>();
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
                    //SelectedCategory = Categories.LastOrDefault();
                    //SelectedCategory = Categories.FirstOrDefault();
                };
            categoryService.RetrieveCategories();            
        }

        #endregion

        #region ICatalogPresentationModel Members


        public void PlaySelectedPreview(Action play)
        {
            var video = SelectedCategory.SelectedCatalog.SelectedVideo ?? SelectedCategory.SelectedCatalog.Videos.FirstOrDefault();
            if(video != null)
                video.IsSelected = true;
            play();
        }

        #endregion
    }
}
