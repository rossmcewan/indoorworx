using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class MeasurementClassMap : ClassMap<Measurement>
    {
        public MeasurementClassMap()
        {
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.Name).Unique().Not.Nullable();
            References(x => x.UnitOfMeasure).Not.Nullable();
            Map(x => x.Value).Unique().Not.Nullable();
            HasMany(x => x.TrainingZones).Cascade.SaveUpdate();
        }
    }
}
