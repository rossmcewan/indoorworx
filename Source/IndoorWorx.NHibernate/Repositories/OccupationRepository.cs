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
    public class OccupationRepository : RepositoryBase<Occupation>, IOccupationRepository
    {
        public OccupationRepository([Dependency(SessionFactoryNames.IndoorWorx)] ISessionFactory factory) : base(factory) { }
    }
}
