using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class TrainingSetTextClassMap : ClassMap<TrainingSetText>
    {
        public TrainingSetTextClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Animation);
            Map(x => x.Duration);
            Map(x => x.MainText);
            Map(x => x.StartTime);
            Map(x => x.SubText);
        }
    }
}
