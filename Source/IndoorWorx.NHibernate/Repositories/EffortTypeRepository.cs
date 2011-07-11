using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure.Repositories;
using NHibernate;
using IndoorWorx.NHibernate.Constants;
using Microsoft.Practices.Unity;

namespace IndoorWorx.NHibernate.Repositories
{
    public class EffortTypeRepository : RepositoryBase<EffortType>, IEffortTypeRepository
    {
        public EffortTypeRepository([Dependency(SessionFactoryNames.IndoorWorx)] ISessionFactory factory) : base(factory) { }
    }
}
