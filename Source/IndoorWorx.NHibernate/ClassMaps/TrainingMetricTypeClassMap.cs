using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class TrainingMetricTypeClassMap : ClassMap<TrainingMetricType>
    {
        public TrainingMetricTypeClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Title);
            Map(x => x.Description);
            Map(x => x.Calculator);
            Map(x => x.IsActive);
        }
    }
}
