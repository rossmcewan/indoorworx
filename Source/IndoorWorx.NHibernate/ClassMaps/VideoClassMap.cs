using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;
using IndoorWorx.NHibernate.UserTypes;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class VideoClassMap : ClassMap<Video>
    {
        public VideoClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Title).Not.Nullable();
            Map(x => x.Description);
            Map(x => x.Created);
            Map(x => x.CreatedBy);
            Map(x => x.Modified);
            Map(x => x.ModifiedBy);
            Map(x => x.Sequence);
            Map(x => x.ImageUri).CustomType<UriType>();
            Map(x => x.StreamUri).CustomType<UriType>().Not.Nullable();
            Map(x => x.TelemetryUri).CustomType<UriType>();
            Map(x => x.Duration);
            Component(x => x.TrainingMetrics, c =>
                {
                    c.Map(y => y.IntensityFactor);
                    c.Map(y => y.NormalizedPower);
                    c.Map(y => y.VariabilityIndex);
                    c.Map(y => y.AveragePower);
                });
            HasMany(x => x.TrainingSets).KeyColumn("Parent").Cascade.SaveUpdate().Fetch.Subselect();
            HasMany(x => x.Reviews).KeyColumn("Video").Cascade.SaveUpdate().Fetch.Subselect();
        }
    }
}
