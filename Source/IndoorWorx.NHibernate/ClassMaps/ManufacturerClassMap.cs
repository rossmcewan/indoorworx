using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class ManufacturerClassMap : ClassMap<Manufacturer>
    {
        public ManufacturerClassMap()
        {
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.Name).Not.Nullable().Unique();
        }
    }
}
