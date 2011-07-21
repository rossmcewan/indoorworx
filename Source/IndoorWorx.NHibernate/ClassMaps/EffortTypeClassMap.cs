using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class EffortTypeClassMap : ClassMap<EffortType>
    {
        public EffortTypeClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Title);
            Map(x => x.Description);
            Map(x => x.Tag);
            Map(x => x.Sequence);            
        }
    }
}
