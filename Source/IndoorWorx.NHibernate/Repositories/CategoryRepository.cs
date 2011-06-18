using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure.Repositories;
using Microsoft.Practices.Unity;
using IndoorWorx.NHibernate.Constants;
using NHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;

namespace IndoorWorx.NHibernate.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository([Dependency(SessionFactoryNames.IndoorWorx)] ISessionFactory factory) : base(factory) { }

        public override ICollection<Category> FindAll(Expression<Func<Category, bool>> where)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var result = session.Query<Category>()
                    .Where(where == null ? x => true : where)
                    .FetchMany(x => x.Catalogs)
                    .ThenFetchMany(x => x.Videos)
                    .ThenFetchMany(x=>x.Reviews);
                var categories = result.ToList();
                foreach (var category in categories)
                {
                    category.Catalogs = category.Catalogs.Distinct().OrderBy(x=>x.Sequence).ToList();
                    foreach (var catalog in category.Catalogs)
                    {
                        catalog.Videos = catalog.Videos.Distinct().OrderBy(x => x.Sequence).ToList();                        
                    }
                }
                return categories;
            }            
        }
    }
}
