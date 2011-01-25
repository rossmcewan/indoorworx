using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class SocialMediaNotificationClassMap : ClassMap<SocialMediaNotification>
    {
        public SocialMediaNotificationClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Name).Unique().Not.Nullable();
        }
    }
}
