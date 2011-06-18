using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class TrainingMetricClassMap : ClassMap<TrainingMetric>
    {
        public TrainingMetricClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            References(x => x.Type).Not.Nullable();
            Map(x => x.Value);
        }
    }
}
