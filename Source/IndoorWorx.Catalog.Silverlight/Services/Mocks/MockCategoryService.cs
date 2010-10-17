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

namespace IndoorWorx.Catalog.Services.Mocks
{
    public class MockCategoryService : ICategoryService
    {
        #region ICategoryService Members

        public event EventHandler<DataEventArgs<ICollection<Category>>> CategoriesRetrieved;

        public event EventHandler<DataEventArgs<Exception>> CategoryRetrievalError;

        public void RetrieveCategories()
        {
            List<Category> categories = new List<Category>();
            categories.Add(new Category()
            {
                Title = "Cycling",
                Catalogs = new List<Infrastructure.Models.Catalog>()
                {
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton",
                        Children = new List<Infrastructure.Models.Catalog>()
                {
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    }
                }
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    },
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://http://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg/636px-Robbie_McEwen_2007_Bay_Cycling_Classic_2.jpg", UriKind.Absolute),
                        Title = "The Peleton"                       
                    }
                }
            });
            categories.Add(new Category()
            {
                Title = "Rowing",
                Catalogs = new List<Infrastructure.Models.Catalog>()
                {
                    new Infrastructure.Models.Catalog()
                    {
                        Image = new Uri("http://www.sportindustry.biz/resource/binary/cache/85df6bb0d9d1209c60fe1d043b5fe084/568x300_rowing2_n.jpg", UriKind.Absolute),
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
