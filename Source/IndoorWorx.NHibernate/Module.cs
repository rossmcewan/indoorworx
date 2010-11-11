using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Practices.Unity;
using NHibernate;
using IndoorWorx.NHibernate.Constants;
using NHibernate.Tool.hbm2ddl;
using IndoorWorx.Infrastructure.Repositories;
using IndoorWorx.NHibernate.Repositories;

namespace IndoorWorx.NHibernate
{
    public class Module : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        #region IModule Members

        public void Initialize()
        {
            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(x => x.FromConnectionStringWithKey("IndoorWorx")))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Module>())
                .BuildSessionFactory();
            Container.RegisterInstance<ISessionFactory>(SessionFactoryNames.IndoorWorx, sessionFactory);

            Container.RegisterInstance<ICatalogRepository>(Container.Resolve<CatalogRepository>());
            Container.RegisterInstance<ICategoryRepository>(Container.Resolve<CategoryRepository>());
            Container.RegisterInstance<IVideoRepository>(Container.Resolve<VideoRepository>());
        }

        #endregion
    }
}
