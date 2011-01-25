using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class ColourClassMap : ClassMap<Colour>
    {
        public ColourClassMap()
        {
            Id(x => x.Id).GeneratedBy.Guid();
            Map(x => x.Alpha);
            Map(x => x.Red);
            Map(x => x.Green);
            Map(x => x.Blue);
        }

    }
}
