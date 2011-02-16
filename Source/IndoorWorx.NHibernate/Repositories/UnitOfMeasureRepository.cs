using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure.Repositories;
using Microsoft.Practices.Unity;
using NHibernate;
using IndoorWorx.NHibernate.Constants;

namespace IndoorWorx.NHibernate.Repositories
{
    public class UnitOfMeasureRepository :RepositoryBase<UnitOfMeasure> , IUnitOfMeasureRepository
    {
        public UnitOfMeasureRepository([Dependency(SessionFactoryNames.IndoorWorx)] ISessionFactory factory) : base(factory) { }
    }
}
