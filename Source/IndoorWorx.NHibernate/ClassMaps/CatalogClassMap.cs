using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class CatalogClassMap : ClassMap<Catalog>
    {
        public CatalogClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Image);
            Map(x => x.Title).Not.Nullable();
            Map(x => x.Description);
            Map(x => x.Sequence);
            HasMany(x => x.Videos).KeyColumn("Catalog").Cascade.SaveUpdate().Fetch.Subselect();
        }
    }
}
