using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;
using IndoorWorx.NHibernate.UserTypes;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class TrainingZoneClassMap : ClassMap<TrainingZone>
    {
        public TrainingZoneClassMap()
        {
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.Name);
            References(x => x.ColorRepresentation);
            Component(x => x.Range, c =>
            {
                c.Map(y => y.UpperValue);
                c.Map(y => y.LowerValue);
            });
        }
    }
}
