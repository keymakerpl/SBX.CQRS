using Domain.Common;

namespace Domain.Persons.Employees.Developers
{
    public class Developer : Entity<int>
    {
        public WorkTime WorkTime { get; set; }

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