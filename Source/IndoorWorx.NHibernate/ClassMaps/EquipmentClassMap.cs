using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class EquipmentClassMap : ClassMap<Equipment>
    {
        public EquipmentClassMap()
        {
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.Name).Not.Nullable();
            References(x => x.Manufacturer);
            HasManyToMany(x => x.ActivityTypes).Cascade.All();
            HasManyToMany(x => x.EquipmentFeatures).Cascade.All();
        }
    }
}
