﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;
using IndoorWorx.NHibernate.UserTypes;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class TrainingSetClassMap : ClassMap<TrainingSet>
    {
        public TrainingSetClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Polymorphism.Explicit();
            Map(x => x.Title).Not.Nullable();
            Map(x => x.Description);
            Map(x => x.Created);
            Map(x => x.CreatedBy);
            Map(x => x.Modified);
            Map(x => x.ModifiedBy);
            Map(x => x.ImageUri).CustomType<UriType>();
            Map(x => x.StreamUri).CustomType<UriType>().Not.Nullable();
            Map(x => x.Duration);
            Map(x => x.Credits);
            Map(x => x.RideCredits);
            Map(x => x.IsPublic);
            References(x => x.VideoMetadata).Cascade.SaveUpdate().Not.LazyLoad();
            References(x => x.TelemetryInfo).Cascade.SaveUpdate().Not.LazyLoad();
            References(x => x.EffortType).Not.LazyLoad();
            HasMany(x => x.Intervals).Cascade.AllDeleteOrphan().Not.LazyLoad().OrderBy("Sequence").KeyColumn("TrainingSet");
            HasMany(x => x.VideoText).Cascade.AllDeleteOrphan().Not.LazyLoad().KeyColumn("TrainingSet");
        }
    }
}
