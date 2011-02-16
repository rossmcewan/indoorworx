using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class ActivityTypeClassMap : ClassMap<ActivityType>
    {
        public ActivityTypeClassMap()
        {
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.Name).Not.Nullable().Unique();
            Map(x => x.IsActive);
            HasManyToMany(x => x.Equipment)
                .Table("ActivityType_Equipment")
                .ParentKeyColumn("ActivityType")
                .ChildKeyColumn("Equipment")
                .Cascade.All();
        }
    }
}
