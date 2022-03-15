using Domain.Projects;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using System;

namespace Infrastructure.Persistence.Domain.Projects
{
    internal class ProjectMap : ClassMap<Project>
    {
        public ProjectMap()
        {
            Schema("Projects");
            Table("Projects");

            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.DevelopersLimit);
            Map(x => x.CodeLinesToWrite);
            Map(x => x.StartDate);
            Map(x => x.DeadLine);

            Component(x => x.Status, y => y.Map(x => x.Value).Column("Status"));

            HasMany(x => x.Developers)
                .Not.LazyLoad()
                .Cascade
                .All();
        }
    }
}
