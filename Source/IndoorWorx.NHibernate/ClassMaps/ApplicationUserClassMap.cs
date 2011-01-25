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
            Map(x => x.Email).Not.Nullable();
            Map(x => x.Country).Not.Nullable();
            HasMany(x => x.Activities).Cascade.SaveUpdate().Fetch.Subselect();
            HasMany(x => x.SocialProfile).Cascade.SaveUpdate().Fetch.Subselect();
        }
    }
}
