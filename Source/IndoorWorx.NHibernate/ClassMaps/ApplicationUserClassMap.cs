using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class ApplicationUserClassMap : ClassMap<ApplicationUser>
    {
        public ApplicationUserClassMap()
        {
            Id(x => x.Username);
            Map(x => x.Firstname).Not.Nullable();
            Map(x => x.Lastname).Not.Nullable();
            Map(x => x.Gender).Not.Nullable();
            Map(x => x.About).Nullable();
            Map(x => x.Email).Not.Nullable();
            References(x => x.Occupation).Not.Nullable();
            References(x => x.ReferralSource).Not.Nullable();
            Map(x => x.Country).Not.Nullable();
            References(x => x.SportingHabits);
            HasMany(x => x.Activities).KeyColumn("Activity").Cascade.SaveUpdate().Fetch.Subselect();
            HasMany(x => x.SocialProfile).KeyColumn("SocialProfile").Cascade.SaveUpdate().Fetch.Subselect();
        }
    }
}
