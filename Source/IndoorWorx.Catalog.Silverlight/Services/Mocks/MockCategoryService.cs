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
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    ImageUri = new Uri("http://localhost:3415/Mock/tri1.jpg",UriKind.Absolute),
                                    Title = "Rand Waterboard Intervals",
                                    TrainingSets = new List<TrainingSet>()
                                    {
                                        new TrainingSet()
                                        {
                                            Name = "Entire Ride",
                                            Description = "The entire ride entails a quick warm up, followed by 2 by 2 minutes at 120 % FTP with 2 minutes RI; followed by 5 minutes at 110% FTP with 5 minutes RI. We repeat this 5 times before a quick cool down. This is a great set ... dig deep."
                                        },
                                        new TrainingSet()
                                        {
                                            Name = "5 x 5",
                                            Description = "A quick warm up, followed by 5 by 5 minutes at 110% FTP with 5 minutes RI. This is followed by a quick cool down."
                                        },
                                        new TrainingSet()
                                        {
                                            Name = "15 x 2",
                                            Description = "A quick warm up, followed by 15 by 2 minutes at 120% FTP with 2 minutes RI. This is followed by a quick cool down. Prepare to suffer."
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
