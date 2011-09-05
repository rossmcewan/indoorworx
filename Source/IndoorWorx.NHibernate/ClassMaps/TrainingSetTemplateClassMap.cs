using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class TrainingSetTemplateClassMap : ClassMap<TrainingSetTemplate>
    {
        public TrainingSetTemplateClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Title);
            Map(x => x.Description).Length(5000);
            Map(x => x.Duration);
            Map(x => x.Credits);
            Map(x => x.IsPublic);
            References(x => x.EffortType).Not.Nullable().Not.LazyLoad();
            HasMany(x => x.Intervals).Cascade.AllDeleteOrphan().Not.LazyLoad().OrderBy("Sequence").KeyColumn("TrainingSetTemplate");
            HasMany(x => x.VideoText).Cascade.AllDeleteOrphan().Not.LazyLoad().KeyColumn("TrainingSetTemplate").OrderBy("StartTime");
        }
    }
}
