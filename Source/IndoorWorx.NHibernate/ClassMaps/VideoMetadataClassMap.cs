using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class VideoMetadataClassMap : ClassMap<VideoMetadata>
    {
        public VideoMetadataClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.FilmedBy);
            Map(x => x.FilmedWith);
            Map(x => x.WhenFilmed);
        }
    }
}
