using CSharpFunctionalExtensions;
using Domain.Common;
using Domain.SharedKernel;

namespace Domain.Projects
{
    public class Developer : AggregateRoot
    {
        protected Developer() { }

        private Developer(PersonName firstName, PersonName lastName, double codeLinesPerHour, WorkTime workTime)
        {
            FirstName = firstName;
            LastName = lastName;
            CodeLinesPerHour = codeLinesPerHour;
            WorkTime = workTime;
        }

        public static Result<Developer> CreateDeveloper(string firstName,
                                                        string lastName,
                                                        double codeLinesPerHour,
                                                        WorkTime workTime)
        {
            var firstNameResult = PersonName.Create(firstName);
            var lastNameResult = PersonName.Create(lastName);
            var codeLinesResult = Result.SuccessIf(codeLinesPerHour > 0, "Doveloper should code!");
            var workTimeResult = Result.SuccessIf(workTime.HoursPerDay > 0, "Developer should code at least one hour per day!");

            return Result.Combine(firstNameResult, lastNameResult, codeLinesResult, workTimeResult)
                         .Map(() => new Developer(firstNameResult.Value, lastNameResult.Value, codeLinesPerHour, workTime));
        }

        public virtual Project Project { get; protected set; }
        public virtual PersonName FirstName { get; }
        public virtual PersonName LastName { get; }
        public virtual WorkTime WorkTime { get; }
        public virtual double CodeLinesPerHour { get; }
    }
}