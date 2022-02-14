using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Utils
{
    public interface IExpectHolidayOnBuilder
    {
        IHolidays Build();

        IExpectHolidayOnBuilder OnDay(int month, int day);

        IExpectHolidayOnBuilder OnDay(Func<int, DateOnly> when);

        IExpectHolidayOnBuilder OnDayWhen(int month, int day, Func<DateOnly, bool> when);
    }

    public interface IHolidaysBuilder
    {
        IHolidays Build();
    }

    public sealed class HolidaysBuilder :
        IHolidaysBuilder,
        IExpectHolidayOnBuilder
    {
        private readonly Dictionary<DateOnly, Func<DateOnly, bool>> holidays = new();
        private readonly int year;

        private HolidaysBuilder(int year) => this.year = year;

        public static IExpectHolidayOnBuilder ForYear(int year) => new HolidaysBuilder(year);

        public IHolidays Build() =>
                    new Holidays(holidays.Where(kvp => kvp.Value(kvp.Key)).Select(kvp => kvp.Key));

        public IExpectHolidayOnBuilder OnDay(int month, int day)
        {
            holidays.Add(new DateOnly(year, month, day), _ => true);
            return this;
        }

        public IExpectHolidayOnBuilder OnDay(Func<int, DateOnly> whenYear)
        {
            holidays.Add(whenYear(year), _ => true);
            return this;
        }

        public IExpectHolidayOnBuilder OnDayWhen(int month, int day, Func<DateOnly, bool> when)
        {
            holidays.Add(new DateOnly(year, month, day), when);
            return this;
        }
    }
}