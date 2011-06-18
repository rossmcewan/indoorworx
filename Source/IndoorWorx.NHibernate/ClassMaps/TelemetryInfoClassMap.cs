using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.NHibernate.UserTypes;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class TelemetryInfoClassMap : ClassMap<TelemetryInfo>
    {
        public TelemetryInfoClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.RecordingInterval).Not.Nullable();
            Map(x => x.TelemetryUri).CustomType<UriType>().Not.Nullable();
        }
    }
}
