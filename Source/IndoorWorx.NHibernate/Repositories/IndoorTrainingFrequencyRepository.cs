﻿using System;
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
    public class IndoorTrainingFrequencyRepository : RepositoryBase<IndoorTrainingFrequency>, IIndoorTrainingFrequencyRepository
    {
        public IndoorTrainingFrequencyRepository([Dependency(SessionFactoryNames.IndoorWorx)] ISessionFactory factory) : base(factory) { }
    }
}