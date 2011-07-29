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
            Map(x => x.Duration).Not.Nullable();
            Map(x => x.Title);
            Map(x => x.Description);
            Map(x => x.Effort);
            Map(x => x.Sequence).Not.Nullable();
            Map(x => x.TemplateSection);
            Map(x => x.SectionGroup);
            References(x => x.EffortType).Not.Nullable().Not.LazyLoad();
            References(x => x.IntervalType).Not.LazyLoad();
            References(x => x.IntervalLevel).Not.Nullable().Not.LazyLoad();
        }
    }
}
