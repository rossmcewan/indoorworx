using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class IntervalLevelClassMap : ClassMap<IntervalLevel>
    {
        public IntervalLevelClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Title).Not.Nullable();
            Map(x => x.MaximumPercentageOfFtp);
            Map(x => x.MinimumPercentageOfFtp);
            Map(x => x.MinimumPercentageOfFthr);
            Map(x => x.MaximumPercentageOfFthr);
            Map(x => x.MinRPE);
            Map(x => x.MaxRPE);
            Map(x => x.TypicalMaxDuration);
            Map(x => x.TypicalMinDuration);
        }
    }
}
