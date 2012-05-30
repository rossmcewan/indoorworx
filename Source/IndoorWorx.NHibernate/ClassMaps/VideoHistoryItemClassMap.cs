using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class VideoHistoryItemClassMap : ClassMap<VideoHistoryItem>
    {
        public VideoHistoryItemClassMap()
        {
            Id(x => x.Id);
            Map(x => x.Comments);
            Map(x => x.Finished);
            Map(x => x.FinishType).CustomType<FinishType>();
            Map(x => x.PlayFrom);
            Map(x => x.PlayTo);
            Map(x => x.Rating);
            Map(x => x.Started);
            References(x => x.Video).Cascade.None();
        }
    }
}
