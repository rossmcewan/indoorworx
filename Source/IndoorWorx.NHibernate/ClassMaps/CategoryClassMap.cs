using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;
using IndoorWorx.NHibernate.UserTypes;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class CategoryClassMap : ClassMap<Category>
    {
        public CategoryClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();            
            Map(x => x.Description);
            Map(x => x.Title).Not.Nullable();
            Map(x => x.Sequence);
            Map(x => x.CatalogUri).CustomType<UriType>();
            HasMany(x => x.Catalogs).KeyColumn("Category").Cascade.SaveUpdate().Fetch.Subselect();
        }
    }
}
