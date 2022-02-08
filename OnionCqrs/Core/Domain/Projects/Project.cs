using CSharpFunctionalExtensions;
using Domain.Common;
using Domain.Persons.Employees.Developers;
using System;
using System.Linq;

namespace Domain.Projects
{
    public class Project : AggregeteRoot<int>
    {
        private readonly DateOnly deadLine;
        private readonly int developersLimit;
        private readonly int codeLinesToWrite;

        private DevelopersCollection Developers { get; } = new DevelopersCollection();

        public Project(DateOnly deadLine, int developersLimit, int codeLinesToWrite)
        {
            this.deadLine = deadLine;
            this.developersLimit = developersLimit;
            this.codeLinesToWrite = codeLinesToWrite;
        }

        public int GetProjectDevelopersCount() => this.Developers.Count();

        public Result CanAssignDeveloper(Developer developer) =>
            Result.SuccessIf(this.Developers.Count() < this.developersLimit,
                $"Developers limit for this project exceed limit of {this.developersLimit}");

        public Result AssignDeveloper(Developer developer) =>
            CanAssignDeveloper(developer)
            .Bind(() => this.Developers.Add(developer));

        public Result UnassignDeveloper(Developer developer) =>
            this.Developers.Remove(developer);

        public DateOnly CalculateEstimatedDateOfComplete(DateOnly startDate) =>
            Result.Success(Developers.CalculateEstimatedCodeLinesPerHour())
                  .Map(codeLinesPerHour => codeLinesToWrite / codeLinesPerHour)
                  .Map(hoursToComplete => startDate.AddDays(Convert.ToInt32(Math.Round(hoursToComplete / 24, MidpointRounding.ToZero))))
                  .Map(dateOfComplete => new DateOnly(dateOfComplete.Year, dateOfComplete.Month, dateOfComplete.Day))
                  .Finally(dateToReturn => dateToReturn.Value);
    }
}
