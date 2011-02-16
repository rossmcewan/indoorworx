using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class IndoorTrainingFrequencyClassMap : ClassMap<IndoorTrainingFrequency>
    {
        public IndoorTrainingFrequencyClassMap()
        {
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.Description).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();
        }
    }
}
