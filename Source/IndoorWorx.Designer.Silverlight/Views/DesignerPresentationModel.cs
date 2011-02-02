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
using IndoorWorx.Designer.Models;
using IndoorWorx.Library.Controls;
using IndoorWorx.Designer.Events;
using IndoorWorx.Infrastructure.Events;
using IndoorWorx.Infrastructure.Navigation;
using IndoorWorx.Designer.Navigation;
using System.Threading;

namespace IndoorWorx.Designer.Views
{
    public class DesignerPresentationModel : BaseModel, IDesignerPresentationModel, ICategoryTreeControlModel
    {
        public event EventHandler EntriesChanged;

        public event EventHandler CategoriesLoaded;

        public event EventHandler<DataEventArgs<Video>> VideoSelected;

        public event EventHandler<DataEventArgs<TrainingSet>> TrainingSetSelected;

        private readonly IServiceLocator serviceLocator;
        private readonly IEventAggregator eventAggregator;
        public DesignerPresentationModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            this.serviceLocator = serviceLocator;
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<AddDesignEntryEvent>().Subscribe(AddEntry);
            eventAggregator.GetEvent<UseSelectedVideoEvent>().Subscribe(UseSelectedVideo);
            this.CatalogContextMenuItems.Add(serviceLocator.GetInstance<UseSelectedVideoMenuItem>());

            RemoveEntryCommand = new DelegateCommand<TrainingSetDesignEntry>(RemoveEntry);
        }

        private ICollection<Category> allCategories;
        public ICollection<Category> AllCategories
        {
            get { return allCategories; }
            set
            {
                allCategories = value;
            }
        }

        public void AddDesignerForSelectedVideo() 
        {
            View.AddDesigner(this.SelectedVideo);
        }

        public void UseSelectedVideo(Video video)
        {
            View.AddDesigner(video);
        }

        public void AddEntry(IDesignerSelectorPresentationModel trainingSetDesign)
        {
            var entry = new TrainingSetDesignEntry()
            {
                Source = trainingSetDesign.Source.SelectedTrainingSet,
                TimeStart = TimeSpan.FromSeconds(trainingSetDesign.SelectionStart.GetValueOrDefault()),
                TimeEnd = TimeSpan.FromSeconds(trainingSetDesign.SelectionEnd.GetValueOrDefault())
            };
            entry.IntensityFactorChanged += (sender, e) =>
                {
                    this.Telemetry = GetDesignedTelemetry();
                };
            Entries.Add(entry);
            this.Telemetry = GetDesignedTelemetry();
            FirePropertyChanged("Entries");
        }

        void View_EntriesChangedOnView(object sender, EventArgs e)
        {
            IsBusy = false;
            View.EntriesChangedOnView -= View_EntriesChangedOnView;
        }

        public IDesignerView View { get; set; }              

        public void SelectVideoWithId(Guid id) 
        {
            //find the video
            foreach (var cat in View.Model.Categories)
            {
                foreach (var catalog in cat.Catalogs)
                {
                    foreach (var video in catalog.Videos)
                    {
                        if (id == video.Id)
                        {
                            SelectedVideo = video;
                            break;
                        }
                    }
                }
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
            }
        }

        private TrainingSet selectedTrainingSet;
        public TrainingSet SelectedTrainingSet
        {
            get { return selectedTrainingSet; }
            set
            {
                selectedTrainingSet = value;
                FirePropertyChanged("SelectedTrainingSet");
                if (VideoSelected != null)
                    TrainingSetSelected(this, new DataEventArgs<TrainingSet>(value));
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
                AllCategories = new List<Category>(e.Value);
                Categories = e.Value;
                if (CategoriesLoaded != null)
                    CategoriesLoaded(this, EventArgs.Empty);
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

        private ICollection<Telemetry> telemetry;
        public ICollection<Telemetry> Telemetry
        {
            get { return telemetry; }
            set
            {
                telemetry = value;
                FirePropertyChanged("Telemetry");
            }
        }

        public ICollection<Telemetry> GetDesignedTelemetry()
        {
            List<Telemetry> result = new List<Telemetry>();
            double seconds = 0;
            foreach (var entry in Entries)
            {
                var entriesToAdd = entry.Source.Telemetry.Where(x =>
                        x.TimePosition.TotalSeconds >= entry.TimeStart.TotalSeconds &&
                        x.TimePosition.TotalSeconds <= entry.TimeEnd.TotalSeconds).Select(x =>
                            {
                                var t = x.Clone();
                                t.PercentageThreshold *= entry.IntensityFactor;
                                return t;
                            }).ToList();
                foreach (var eta in entriesToAdd)
                {
                    seconds += entry.Source.RecordingInterval;
                    eta.TimePosition = TimeSpan.FromSeconds(seconds);
                }
                result.AddRange(entriesToAdd);
            }
            return result;
        }

        private ICollection<TrainingSetDesignEntry> entries = new ObservableCollection<TrainingSetDesignEntry>();
        public ICollection<TrainingSetDesignEntry> Entries
        {
            get { return entries; }
            set
            {
                entries = value;
                FirePropertyChanged("Entries");
            }
        }

        private TrainingSetDesignEntry selectedEntry;
        public TrainingSetDesignEntry SelectedEntry
        {
            get { return selectedEntry; }
            set
            {
                selectedEntry = value;
                FirePropertyChanged("SelectedEntry");
            }
        }

        private ICommand removeEntryCommand;
        public ICommand RemoveEntryCommand
        {
            get { return removeEntryCommand; }
            set
            {
                removeEntryCommand = value;
                FirePropertyChanged("RemoveEntryCommand");
            }
        }

        public void RemoveEntry(TrainingSetDesignEntry entry)
        {
            Entries.Remove(entry);
            this.Telemetry = GetDesignedTelemetry();
            FirePropertyChanged("Entries");
            if (EntriesChanged != null)
                EntriesChanged(this, EventArgs.Empty);
        }

        private ICollection<IMenuItem> contextMenuItems = new ObservableCollection<IMenuItem>();
        public ICollection<Infrastructure.Navigation.IMenuItem> CatalogContextMenuItems
        {
            get
            {
                return this.contextMenuItems;
            }
            set
            {
                this.contextMenuItems.Clear();
                foreach (var item in value)
                    this.contextMenuItems.Add(item);
            }
        }

        #region ICategoryTreeControlModel Members

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

        private object selectedItem;
        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                //if (selectedItem is Category)
                    //SelectedCategory = value as Category;
                if (selectedItem is IndoorWorx.Infrastructure.Models.Catalog)
                {
                    var catalog = selectedItem as IndoorWorx.Infrastructure.Models.Catalog;
                    foreach (var category in Categories)
                    {
                        foreach (var c in category.Catalogs)
                        {
                            if (c.Id == catalog.Id)
                            {
                                //SelectedCategory = category;
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
                                    //SelectedCategory = category;
                                    category.SelectedCatalog = c;
                                    c.SelectedVideo = v;
                                    break;
                                }
                            }
                        }
                    }
                }
                FirePropertyChanged("SelectedItem");
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

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                //need to build a new collection - only adding the ones that meet the search criteria
                if (string.IsNullOrWhiteSpace(value))
                    Categories = AllCategories.ToList();
                else
                {
                    if (value != searchText)
                    {
                        eventAggregator.GetEvent<CatalogSearchTextChangedEvent>().Publish(value);
                        FilterValidSearchResults(value);
                    }
                }
                searchText = value;
                FirePropertyChanged("SearchText");
            }
        }

        private void FilterValidSearchResults(string searchText)
        {
            var validSearchResults = new List<Category>();
            foreach (var category in AllCategories)
            {
                var newCategory = new Category() { Title = category.Title };
                foreach (var catalog in category.Catalogs)
                {
                    var newCatalog = new IndoorWorx.Infrastructure.Models.Catalog() { Title = catalog.Title, Sequence = catalog.Sequence };
                    foreach (var video in catalog.Videos)
                    {
                        if (video.Title.IndexOf(searchText.ToLower(), StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            newCatalog.Videos.Add(video);
                        }
                    }
                    if (newCatalog.Videos.Any())
                        newCategory.Catalogs.Add(newCatalog);
                }
                if (newCategory.Catalogs.Any())
                    validSearchResults.Add(newCategory);
            }
            Categories = validSearchResults;
        }

        #endregion        
    }
}
