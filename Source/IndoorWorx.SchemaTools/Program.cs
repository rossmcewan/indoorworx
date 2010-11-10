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
                            Description = "Training videos filmed by us on our favourite training routes with our favourite training buddies.",
                            Videos = new List<Video>()
                            {
                                new Video()
                                {
                                    Created = DateTime.Now,
                                    CreatedBy = typeof(Program).Assembly.FullName,
                                    ImageUri = new Uri("http://localhost:3415/Mock/tri1.jpg",UriKind.Absolute),                                    
                                    StreamUri = new Uri("http://smoothhd.mp.advection.net/mp/indoorworx/_dld/FILE0001.ism/Manifest", UriKind.Absolute),
                                    Title = "Rand Waterboard - 2s and 5s",
                                    TrainingSets = new List<Video>()
                                    {
                                        new Video()
                                        {
                                            Title = "Entire Ride",
                                            Description = "The entire ride entails a quick warm up, followed by 2 by 2 minutes at 120 % FTP with 2 minutes RI; followed by 5 minutes at 110% FTP with 5 minutes RI. We repeat this 5 times before a quick cool down. This is a great set ... dig deep.",
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
