using Application.Common;
using CSharpFunctionalExtensions;

namespace Application.Domain.Projects.Commands.RegisterDeveloper
{
    public class RegisterDeveloperCommand : ICommand<Result<DeveloperDto>>
    {
        public RegisterDeveloperCommand(string firstName,
                                        string lastName,
                                        double codeLinesPerHour,
                                        double workTime)
        {
            FirstName = firstName;
            LastName = lastName;
            CodeLinesPerHour = codeLinesPerHour;
            WorkTime = workTime;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public double CodeLinesPerHour { get; }
        public double WorkTime { get; }
    }
}