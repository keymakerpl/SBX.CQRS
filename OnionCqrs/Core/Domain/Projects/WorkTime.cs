using Domain.Common;
using System.Collections.Generic;

namespace Domain.Projects
{
    public sealed class WorkTime : ValueObject
    {
        public static readonly WorkTime FullTimeWorker = new(8);
        public static readonly WorkTime PartTimeWorker = new(4);
        public static readonly WorkTime None = new(0);

        private WorkTime() { }

        private WorkTime(double hoursPerDay) => this.HoursPerDay = hoursPerDay;

        public double HoursPerDay { get; }

        public static WorkTime FromHours(double workTime) => new WorkTime(workTime);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return HoursPerDay;
        }
    }
}