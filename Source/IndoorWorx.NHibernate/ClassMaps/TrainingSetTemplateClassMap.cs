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
            Map(x => x.Description);
            Map(x => x.Duration);
            Map(x => x.Credits);
            HasMany(x => x.Intervals).KeyColumn("TrainingSetTemplate").Cascade.All().Not.LazyLoad().OrderBy("Sequence");
        }
    }
}
