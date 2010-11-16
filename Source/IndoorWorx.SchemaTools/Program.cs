using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using IndoorWorx.NHibernate;
using NHibernate.Tool.hbm2ddl;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.SchemaTools
{
    class Program
    {
        static void Main(string[] args)
        {
            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(x => x.FromConnectionStringWithKey("IndoorWorx")))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Module>())
                .ExposeConfiguration(x => new SchemaExport(x).SetOutputFile("../../Schema/createdb.sql").Create(true, true))
                .BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                var category = new Category()
                {
                    Title = "Cycling",
                    Description = "Indoor cycling training videos.",
                    Catalogs = new List<Catalog>()
                    {
                        new Catalog()
                        {
                            Title = "IndoorWorx",
                            Sequence = 1,
                            Description = "Training videos filmed by us on our favourite training routes with our favourite training buddies.",
                            Videos = new List<Video>()
                            {
                                new Video()
                                {
                                    Sequence = 1,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/randwaterboard.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Rand Waterboard - 2s and 5s",
                                    TrainingSets = new List<Video>()
                                    {
                                        new Video()
                                        {
                                            Sequence = 1,
                                            Title = "Entire Ride",
                                            Description = "The entire ride entails a quick warm up, followed by 2 by 2 minutes at 120 % FTP with 2 minutes RI; followed by 5 minutes at 110% FTP with 5 minutes RI. We repeat this 5 times before a quick cool down. This is a great set ... dig deep.",
                                            StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute)
                                        },
                                        new Video()
                                        {
                                            Sequence = 2,
                                            Title = "5 x 5",
                                            Description = "The ride entails a quick warm up, followed by 5 by 5 minutes at 110% FTP with 5 minutes RI; this is followed by a quick cool down.",
                                            StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute)
                                        }
                                    }
                                },
                                new Video()
                                {
                                    Sequence = 2,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/suikerbosrand.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Suikerbosrand Nature Reserve",
                                    TrainingSets = new List<Video>()
                                    {
                                        new Video()
                                        {
                                            Sequence = 1,
                                            Title = "Entire Ride",
                                            Description = "This is a ride through the Suikerbosrand Nature Reserve. About two and a half hours with some good climbing. Go hard up the climbs, there's recovery on the other side. Keep your eyes peeled for eland and baboon!",
                                            StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute)
                                        }
                                    }
                                }
                            }
                        },
                        new Catalog()
                        {
                            Title = "MTN Energade",
                            ImageUri = new Uri("http://localhost:3415/Mock/mtn_energade.jpg", UriKind.Absolute),
                            Sequence = 2,
                            Description = "Training videos filmed by the MTN Energade cycling team.",
                            Videos = new List<Video>()
                            {
                                new Video()
                                {
                                    Sequence = 1,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/cyclechallenge.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Pick n Pay 94.7",
                                    TrainingSets = new List<Video>()
                                    {
                                        new Video()
                                        {
                                            Sequence = 1,
                                            Title = "The Race",
                                            Description = "Ride with the men's elite champions of the MTN Energade cycling team doing Jo'burg's toughest race! Suffer with them on the break, close down the attacks, get yourself in a position to win the sprint! Suffer like the pros!",
                                            StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                            Reviews = new List<VideoReview>()
                                            {
                                                new VideoReview()
                                                {
                                                    Created = DateTime.Now,
                                                    CreatedBy = "ross.mcewan",
                                                    Rating = 4,
                                                    Title = "What a great ride!",
                                                    Comment = "I loved this ride! It really felt like I was in the middle of the action!"
                                                },
                                                new VideoReview()
                                                {
                                                    Created = DateTime.Now,
                                                    CreatedBy = "dianne.emery",
                                                    Rating = 5,
                                                    Title = "Awesome!",
                                                    Comment = "Ride was great! Felt like I was one of the pros going for the win! Loved it!"
                                                }
                                            }
                                        }
                                    }
                                },
                                new Video()
                                {
                                    Sequence = 2,
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/cape-argus.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Pick n Pay Cape Argus",
                                    TrainingSets = new List<Video>()
                                    {
                                        new Video()
                                        {
                                            Sequence = 1,
                                            Title = "The Race",
                                            Description = "Ride with the men's elite champions of the MTN Energade cycling team racing the world's biggest race! Suffer up the climbs, hang with them on the descent, set yourself up for the win!",
                                            StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute)
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(category);
                    transaction.Commit();
                }
            }
        }
    }
}
