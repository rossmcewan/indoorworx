using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Repositories;
using NHibernate;
using NHibernate.Linq;

namespace IndoorWorx.NHibernate.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly ISessionFactory sessionFactory;
        public RepositoryBase(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        #region IRepository<T> Members

        public T Get(object id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public ICollection<T> FindAll(params System.Linq.Expressions.Expression<Func<T, bool>>[] where)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var result = from t in session.Query<T>() select t;
                foreach (var whereClause in where)
                {
                    result = result.Where(whereClause);
                }
                return result.ToList();
            }
        }

        public T FindOne(params System.Linq.Expressions.Expression<Func<T, bool>>[] where)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var result = from t in session.Query<T>() select t;
                foreach (var whereClause in where)
                {
                    result = result.Where(whereClause);
                }
                return result.SingleOrDefault();
            }
        }

        public T FindFirst(params System.Linq.Expressions.Expression<Func<T, bool>>[] where)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var result = from t in session.Query<T>() select t;
                foreach (var whereClause in where)
                {
                    result = result.Where(whereClause);
                }
                return result.FirstOrDefault();
            }
        }

        public T Save(T entity)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(entity);
                    transaction.Commit();
                }
            }
            return entity;
        }

        #endregion
    }
}
