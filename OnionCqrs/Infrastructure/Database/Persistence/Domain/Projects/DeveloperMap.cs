using Domain.Projects;
using Domain.SharedKernel;
using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace Infrastructure.Persistence.Domain.Projects
{
    internal class DeveloperMap : ClassMap<Developer>
    {
        public DeveloperMap()
        {
            Schema("Projects");
            Table("Developers");

            Id(x => x.Id);

            Map(x => x.CodeLinesPerHour).Not.Nullable();

            Component(x => x.FirstName, y => y.Map(Reveal.Member<PersonName>("value")).Column("FirstName").Not.Nullable());
            Component(x => x.LastName, y => y.Map(Reveal.Member<PersonName>("value")).Column("LastName").Not.Nullable());
            Component(x => x.WorkTime, y => y.Map(x => x.HoursPerDay).Column("HoursPerDay").Not.Nullable());

            References(x => x.Project).Column("ProjectId").ForeignKey("Id");
        }
    }
}
