using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Repositories;
using NHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;

namespace IndoorWorx.NHibernate.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly ISessionFactory sessionFactory;
        public RepositoryBase(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        #region IRepository<T> Members

        public virtual T Get(object id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public virtual ICollection<T> FindAll(Expression<Func<T, bool>> where)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var result = from t in session.Query<T>() select t;
                if(where != null)
                {
                    result = result.Where(where);
                }
                return result.ToList();
            }
        }

        public virtual T FindOne(Expression<Func<T, bool>> where)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var result = from t in session.Query<T>() select t;
                if (where != null)
                {
                    result = result.Where(where);
                }
                return result.SingleOrDefault();
            }
        }

        public virtual T FindFirst(Expression<Func<T, bool>> where)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var result = from t in session.Query<T>() select t;
                if (where != null)
                {
                    result = result.Where(where);
                }
                return result.FirstOrDefault();
            }
        }

        public virtual T Save(T entity)
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
