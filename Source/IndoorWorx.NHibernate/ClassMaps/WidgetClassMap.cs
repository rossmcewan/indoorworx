using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class WidgetClassMap : ClassMap<Widget>
    {
        public WidgetClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Image);
            Map(x => x.Title);
            Map(x => x.ContentRenderer);
        }
    }
}
