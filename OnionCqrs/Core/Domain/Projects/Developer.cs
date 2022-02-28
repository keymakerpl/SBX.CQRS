using Domain.Common;
using Domain.Utils;

namespace Domain.Projects
{
    public class Developer : Entity<int>
    {
        public WorkTime WorkTime { get; }

        public Developer(PersonName firstName, PersonName lastName, double codeLinesPerHour, WorkTime workTime)
        {
            FirstName = firstName;
            LastName = lastName;
            CodeLinesPerHour = codeLinesPerHour;
            WorkTime = workTime;
        }

        public double CodeLinesPerHour { get; }
        public PersonName FirstName { get; }
        public PersonName LastName { get; }
    }
}