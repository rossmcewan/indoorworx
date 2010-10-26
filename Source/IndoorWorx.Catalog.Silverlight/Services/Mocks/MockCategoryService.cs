using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Composite.Events;
using System.Collections.Generic;
using IndoorWorx.Infrastructure.Models;
using System.Collections.ObjectModel;

namespace IndoorWorx.Catalog.Services.Mocks
{
    public class MockCategoryService : ICategoryService
    {
        #region ICategoryService Members

        public event EventHandler<DataEventArgs<ICollection<Category>>> CategoriesRetrieved;

        public event EventHandler<DataEventArgs<Exception>> CategoryRetrievalError;

        public void RetrieveCategories()
        {
            ObservableCollection<Category> categories = new ObservableCollection<Category>();
            categories.Add(new Category()
            {
                Title = "Cycling",
                Catalogs = new ObservableCollection<Infrastructure.Models.Catalog>()
                {
                    new Infrastructure.Models.Catalog()
                    {
                        Title = "The Peleton",
                        Videos = new ObservableCollection<Infrastructure.Models.Video>()
                            {
                                new Infrastructure.Models.Video()
                                {
                                    StreamUri = new Uri("http://www.indoorworx.com/media/FILE0001.ism/manifest",UriKind.Absolute),
                                    ImageUri = new Uri("http://localhost:3415/Mock/tri1.jpg",UriKind.Absolute),
                                    Title = "Double Century 2010",
                                    TrainingSets = new List<TrainingSet>()
                                    {
                                        new TrainingSet()
                                        {
                                            Name = "Entire Ride",
                                            Description = "A good 2 hour session through the rolling hills of the Suikerbosrand Nature Reserve. This includes some hard climbs followed by recovery as we go down the other side."
                                        },
                                        new TrainingSet()
                                        {
                                            Name = "2 x 20",
                                            Description = "A good 15 minute warm-up over the rolling roads of the Suikerbosrand Nature Reserve; followed by 2 hard 20 minute intervals performed at 95-105% of FTP, with 5 minute recovery intervals. The cool down is a 10 minute easy pedal down the Rand Waterboard road."
                                        }
                                    }
                                },
                                new Infrastructure.Models.Video()
                                {
                                    StreamUri = new Uri("http://www.indoorworx.com/media/FILE0001.ism/manifest",UriKind.Absolute),
                                    ImageUri = new Uri("http://localhost:3415/Mock/tri2.jpg",UriKind.Absolute),
                                    Title = "Kona 2011"                       
                                },
                                new Infrastructure.Models.Video()
                                {
                                    StreamUri = new Uri("http://www.indoorworx.com/media/FILE0001.ism/manifest",UriKind.Absolute),
                                    ImageUri = new Uri("http://localhost:3415/Mock/tri3.jpg", UriKind.Absolute),
                                    Title = "The Jock 2010"                       
                                }
                            }
                    }
                }
            });
            categories.Add(new Category()
            {
                Title = "Rowing",
                Catalogs = new ObservableCollection<Infrastructure.Models.Catalog>()
                {
                    new Infrastructure.Models.Catalog()
                    {
                        Title = "The Diamond"                       
                    }
                }
            });
            if (CategoriesRetrieved != null)
                CategoriesRetrieved(this, new DataEventArgs<ICollection<Category>>(categories));
        }

        #endregion
    }
}
