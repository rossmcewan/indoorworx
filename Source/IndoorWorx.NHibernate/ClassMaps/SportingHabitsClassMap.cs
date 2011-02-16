using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.NHibernate.ClassMaps
{
    public class SportingHabitsClassMap : ClassMap<SportingHabits>
    {
        public SportingHabitsClassMap()
        {
            Id(x => x.Id);
            References(x => x.CompetitiveLevel).Nullable();
            References(x => x.TrainingVolume).Nullable();
            References(x => x.IndoorTrainingFrequency);
            HasManyToMany(x => x.MySports)
                .Table("SportingHabits_Sport")
                .ParentKeyColumn("SportingHabits")
                .ChildKeyColumn("Sport")
                .Cascade.All();
        }
    }
}
