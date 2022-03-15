using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Projects
{
    public static class DevelopersCollectionExtensions
    {
        public static Result AddDeveloper(this IList<Developer> developers, Developer developer) =>
            Result.Try(() => developers.Add(developer));

        public static Result RemoveDeveloper(this IList<Developer> developers, Developer developer) =>
            Result.Try(() => developers.Remove(developer))
                  .Ensure(isRemoved => isRemoved, "Cannot remove or not found!");

        public static double CalculateEstimatedCodeLinesPerHour(this IList<Developer> developers) =>
            developers.Sum(developer => developer.CodeLinesPerHour);

        public static double CalculateEstimatedCodeLinesPerDay(this IList<Developer> developers) =>
            developers.Sum(developer => developer.WorkTime.HoursPerDay * developer.CodeLinesPerHour);
    }
}
