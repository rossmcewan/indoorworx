using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using FluentNHibernate.Mapping;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class SocialMediaProfileClassMap : ClassMap<SocialMediaProfile>
    {
        public SocialMediaProfileClassMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Username);
            References(x => x.SocialMediaType).Column("SocialMediaType").Not.Nullable();
            Map(x => x.Password);
            HasManyToMany(x => x.NotificationOptions)
                .Table("SocialMediaProfile_SocialMediaNotifications")
                .ParentKeyColumn("SocialMediaProfile")
                .ChildKeyColumn("SocialMediaNotifications")
                .Cascade.SaveUpdate().Fetch.Subselect();
        }
    }
}
