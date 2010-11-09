using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace IndoorWorx.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Get(object id);

        ICollection<T> FindAll(params Expression<Func<T,bool>>[] where);

        T FindOne(params Expression<Func<T, bool>>[] where);

        T FindFirst(params Expression<Func<T, bool>>[] where);

        T Save(T entity);
    }
}
