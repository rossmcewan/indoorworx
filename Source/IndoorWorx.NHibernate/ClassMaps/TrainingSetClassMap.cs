using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.NHibernate.UserTypes;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class TrainingSetClassMap : SubclassMap<TrainingSet>
    {
        public TrainingSetClassMap()
        {
            KeyColumn("Id");
            Map(x => x.TelemetryUri).CustomType<UriType>();
            Map(x => x.RecordingInterval);
            Component(x => x.TrainingMetrics, c =>
            {
                c.Map(y => y.IntensityFactor);
                c.Map(y => y.NormalizedPower);
                c.Map(y => y.VariabilityIndex);
                c.Map(y => y.AveragePower);
            });
        }
    }
}
