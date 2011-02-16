using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Repositories;
using IndoorWorx.Infrastructure.Models;
using Microsoft.Practices.Unity;
using NHibernate;
using IndoorWorx.NHibernate.Constants;

namespace IndoorWorx.NHibernate.Repositories
{
    public class CompetitiveLevelRepository : RepositoryBase<CompetitiveLevel>, ICompetitiveLevelRepository
    {
        public CompetitiveLevelRepository([Dependency(SessionFactoryNames.IndoorWorx)] ISessionFactory factory) : base(factory) { }
    }
}
