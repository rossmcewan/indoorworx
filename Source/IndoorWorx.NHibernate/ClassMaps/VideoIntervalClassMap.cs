using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class VideoIntervalClassMap : ClassMap<VideoInterval>
    {
        public VideoIntervalClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Duration).Not.Nullable();
            Map(x => x.Effort);
            Map(x => x.Sequence).Not.Nullable();
        }
    }
}
