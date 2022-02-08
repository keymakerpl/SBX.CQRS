using CSharpFunctionalExtensions;
using Domain.Persons.Employees.Developers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Projects
{
    public class DevelopersCollection : IEnumerable<Developer>
    {
        private readonly IList<Developer> developers = new List<Developer>();

        public Result Add(Developer developer) =>
            Result.Try(() => developers.Add(developer));

        public Result Remove(Developer developer) =>
            Result.Try(() => developers.Remove(developer))
                  .Ensure(isRemoved => isRemoved, "Cannot remove or not found!");

        public double CalculateEstimatedCodeLinesPerHour() =>
            developers.Sum(developer => developer.CodeLinesPerHour);

        public IEnumerator<Developer> GetEnumerator() => developers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
