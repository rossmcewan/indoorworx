using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class ActivityClassMap : ClassMap<Activity>
    {
        public ActivityClassMap()
        {
            Id(x => x.Id).GeneratedBy.Guid();
            References(x => x.ActivityType).Not.Nullable();
            References(x => x.Equipment);
            HasMany(x => x.Measurements).Cascade.SaveUpdate();
        }
    }
}
