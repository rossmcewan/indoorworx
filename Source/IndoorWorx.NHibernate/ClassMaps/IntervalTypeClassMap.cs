using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class IntervalTypeClassMap : ClassMap<IntervalType>
    {
        public IntervalTypeClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Tag).Not.Nullable();
            Map(x => x.Sequence).Not.Nullable();
            //References(x => x.DefaultLevel).Not.Nullable().Not.LazyLoad();
        }
    }
}
