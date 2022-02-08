using Domain.Common;
using System.Collections.Generic;

namespace Domain.Persons.Employees.Developers
{
    public sealed class WorkTime : ValueObject
    {
        public static readonly WorkTime FullTimeWorker = new(8);
        public static readonly WorkTime PartTimeWorker = new(4);

        private WorkTime(double hoursPerDay) => this.HoursPerDay = hoursPerDay;

        public double HoursPerDay { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return HoursPerDay;
        }
    }
}