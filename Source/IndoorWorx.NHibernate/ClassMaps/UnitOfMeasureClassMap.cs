using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class UnitOfMeasureClassMap : ClassMap<UnitOfMeasure>
    {
        public UnitOfMeasureClassMap()
        {
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.Name).Unique().Not.Nullable();
        }
    }
}
