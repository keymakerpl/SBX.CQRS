using Domain.Persons;
using Domain.Persons.Employees.Developers;
using Domain.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests.Extensions
{
    internal static class DevelopersCollectionExtensions
    {
        public static DevelopersCollection Seed(this DevelopersCollection developers)
        {
            developers.Add(new Developer(PersonName.Create("Radek").Value,
                                         PersonName.Create("Kurek").Value,
                                         120));
            developers.Add(new Developer(PersonName.Create("Jan").Value,
                                         PersonName.Create("Nowak").Value,
                                         220));

            return developers;
        }
    }
}
