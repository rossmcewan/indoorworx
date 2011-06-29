using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class VideoIntervalClassMap : SubclassMap<VideoInterval>
    {
        public VideoIntervalClassMap()
        {
            KeyColumn("Id");
            Map(x => x.StartTimestamp).Not.Nullable();
            Map(x => x.EndTimestamp).Not.Nullable();
        }
    }
}
