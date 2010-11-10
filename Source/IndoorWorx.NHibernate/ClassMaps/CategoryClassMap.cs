using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class CategoryClassMap : ClassMap<Category>
    {
        public CategoryClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Description);
            Map(x => x.Title).Not.Nullable();
            HasMany(x => x.Catalogs).KeyColumn("Category").Cascade.SaveUpdate();
        }
    }
}
