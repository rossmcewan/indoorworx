using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure.Repositories;
using Microsoft.Practices.Unity;
using IndoorWorx.NHibernate.Constants;
using NHibernate;

namespace IndoorWorx.NHibernate.Repositories
{
    public class IntervalLevelRepository : RepositoryBase<IntervalLevel>, IIntervalLevelRepository
    {
        public IntervalLevelRepository([Dependency(SessionFactoryNames.IndoorWorx)] ISessionFactory factory) : base(factory) { }
    }
}
