using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class IntervalClassMap : ClassMap<Interval>
    {
        public IntervalClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            References(x => x.IntervalType).Not.Nullable();
            Map(x => x.StartTime).Not.Nullable();
            Map(x => x.EndTime).Not.Nullable();
        }
    }
}
