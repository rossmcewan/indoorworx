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
            References(x => x.Manufacturer).Column("Manufacturer");
            HasManyToMany(x => x.EquipmentFeatures)
                .Table("Equipment_EquipmentFeatures")
                .ParentKeyColumn("Equipment")
                .ChildKeyColumn("EquipmentFeatures")
                .Cascade.All();
            Map(x => x.IsActive);
        }
    }
}
