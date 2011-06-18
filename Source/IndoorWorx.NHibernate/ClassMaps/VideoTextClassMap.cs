using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;
using IndoorWorx.Infrastructure.Enums;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class VideoTextClassMap : ClassMap<VideoText>
    {
        public VideoTextClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Animation);
            Map(x => x.Duration);
            Map(x => x.MainText);
            Map(x => x.StartTime);
            Map(x => x.SubText);
            //References(x => x.TrainingSet).Column("TrainingSet");
        }
    }
}
