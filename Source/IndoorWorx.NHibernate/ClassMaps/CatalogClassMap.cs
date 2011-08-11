using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;
using IndoorWorx.NHibernate.UserTypes;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class CatalogClassMap : ClassMap<Catalog>
    {
        public CatalogClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.ImageUri).CustomType<UriType>();
            Map(x => x.Title).Not.Nullable();
            Map(x => x.Description);
            Map(x => x.Sequence);
            References(x => x.Category).Column("Category").Not.LazyLoad();
            HasMany(x => x.Videos).KeyColumn("Catalog").Cascade.SaveUpdate().Fetch.Subselect();
        }
    }
}
