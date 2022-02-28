using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Utils
{
    public interface IExpectNonWorkingWeekDays
    {
        IExpectNonWorkingOnDayBuilder NonWorkingDaysOfWeek(params DayOfWeek[] days);
    }

    public interface IExpectNonWorkingOnDayBuilder
    {
        INonWorkingDays Build();

        IExpectNonWorkingOnDayBuilder OnDay(int month, int day);

        IExpectNonWorkingOnDayBuilder OnDay(Func<int, DateOnly> when);

        IExpectNonWorkingOnDayBuilder OnDayWhen(int month, int day, Func<DateOnly, bool> when);
    }

    public interface IWorkingDaysBuilder
    {
        INonWorkingDays Build();
    }

    public sealed class NonWorkingDaysBuilder :
        IWorkingDaysBuilder,
        IExpectNonWorkingWeekDays,
        IExpectNonWorkingOnDayBuilder
    {
        private readonly Dictionary<DateOnly, Func<DateOnly, bool>> holidays = new();
        private readonly int year;
        private IEnumerable<DayOfWeek> nonWorkingWeekDays;

        private NonWorkingDaysBuilder(int year) => this.year = year;

        public static IExpectNonWorkingWeekDays ForYear(int year) => new NonWorkingDaysBuilder(year);

        public INonWorkingDays Build() =>
                    new NonWorkingDays(holidays.Where(kvp => kvp.Value(kvp.Key)).Select(kvp => kvp.Key), nonWorkingWeekDays);

        public IExpectNonWorkingOnDayBuilder NonWorkingDaysOfWeek(params DayOfWeek[] days)
        {
            nonWorkingWeekDays = new List<DayOfWeek>(days);
            return this;
        }

        public IExpectNonWorkingOnDayBuilder OnDay(int month, int day)
        {
            holidays.Add(new DateOnly(year, month, day), _ => true);
            return this;
        }

        public IExpectNonWorkingOnDayBuilder OnDay(Func<int, DateOnly> whenYear)
        {
            holidays.Add(whenYear(year), _ => true);
            return this;
        }

        public IExpectNonWorkingOnDayBuilder OnDayWhen(int month, int day, Func<DateOnly, bool> when)
        {
            holidays.Add(new DateOnly(year, month, day), when);
            return this;
        }
    }
}