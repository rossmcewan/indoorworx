using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class SocialMediaTypeClassMap : ClassMap<SocialMediaType>
    {
        public SocialMediaTypeClassMap()
        {
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.Name).Unique().Not.Nullable();
        }

    }
}
