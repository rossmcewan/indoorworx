using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class ReviewClassMap : ClassMap<Review>
    {
        public ReviewClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Modified);
            Map(x => x.ModifiedBy);
            Map(x => x.Created);
            Map(x => x.CreatedBy);
            Map(x => x.Comment);
            Map(x => x.Title);
            Map(x => x.Rating);
        }
    }
}
