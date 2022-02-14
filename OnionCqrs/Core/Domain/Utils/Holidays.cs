﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Domain.Utils
{
    public interface IHolidays
    {
        IReadOnlyCollection<DateOnly> All { get; }
    }

    public sealed class Holidays : IHolidays
    {
        public static Func<int, IHolidays> PolandHolidays => year =>
            HolidaysBuilder.ForYear(year)
                           .OnDayWhen(month: 01, day: 01, when: date => date.Year >= 1925)
                           .OnDayWhen(month: 01, day: 06, when: date => date.Year == 1917 || date.Year == 1918 || (date.Year >= 1952 && date.Year <= 1960) || date.Year >= 2011)
                           .OnDayWhen(month: 02, day: 02, when: date => date.Year == 1917 || date.Year == 1918)
                           .OnDayWhen(month: 02, day: 10, when: date => date.Year == 1919)
                           .OnDay(month: 05, day: 01)
                           .OnDayWhen(month: 05, day: 03, when: date => date.Year == 1917 || date.Year == 1918 || (date.Year >= 1919 && date.Year <= 1950) || date.Year >= 1990)
                           .OnDayWhen(month: 05, day: 26, when: date => date.Year == 1917 || date.Year == 1918)
                           .OnDayWhen(month: 07, day: 22, when: date => date.Year >= 1945 && date.Year <= 1989)
                           .OnDayWhen(month: 08, day: 15, when: date => date.Year <= 1960 || date.Year >= 1989)
                           .OnDay(month: 11, day: 01)
                           .OnDayWhen(month: 11, day: 11, when: date => date.Year == 1937 || date.Year == 1938 || date.Year >= 1990)
                           .OnDayWhen(month: 12, day: 08, when: date => date.Year == 1917 || date.Year == 1918)
                           .OnDay(month: 12, day: 25)
                           .OnDay(month: 12, day: 26)
                           .OnDayWhen(month: 12, day: 27, when: date => date.Year == 1917 || date.Year == 1918)
                           .OnDay(when: year => CalculateEasterDate(year))
                           .OnDay(when: year => CalculateEasterDate(year).AddDays(1))
                           .OnDay(when: year => CalculateEasterDate(year).AddDays(49))
                           .OnDay(when: year => CalculateEasterDate(year).AddDays(60))
                           .Build();

        private readonly IEnumerable<DateOnly> dates;

        public IReadOnlyCollection<DateOnly> All =>
            new ReadOnlyCollection<DateOnly>(dates.ToList());

        public Holidays(IEnumerable<DateOnly> dates) => this.dates = dates;

        private static DateOnly CalculateEasterDate(int year)
        {
            int a = year % 19;
            int b = year % 4;
            int c = year % 7;
            int d = ((a * 19) + 24) % 30;
            int e = ((2 * b) + (4 * c) + (6 * d) + 5) % 7;
            if (d == 29 && e == 6) d -= 7;
            if (d == 28 && e == 6 && a > 10) d -= 7;
            return new DateOnly(year, 3, 22).AddDays(d + e);
        }
    }
}
