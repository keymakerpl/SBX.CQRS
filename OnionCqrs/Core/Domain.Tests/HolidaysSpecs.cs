using FluentAssertions;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using static Application.Domain.SharedKernel.NonWorkingDays;

namespace Domain.Tests
{
    public class HolidaysSpecs
    {
        private readonly HashSet<DateOnly> expectedPolandHolidaysForYear2022;

        public HolidaysSpecs()
        {
            this.expectedPolandHolidaysForYear2022 = new HashSet<DateOnly>
            {
                new DateOnly(2022, 01, 01),
                new DateOnly(2022, 01, 06),
                new DateOnly(2022, 04, 17),
                new DateOnly(2022, 04, 18),
                new DateOnly(2022, 05, 01),
                new DateOnly(2022, 05, 03),
                new DateOnly(2022, 06, 05),
                new DateOnly(2022, 06, 16),
                new DateOnly(2022, 08, 15),
                new DateOnly(2022, 11, 01),
                new DateOnly(2022, 11, 11),
                new DateOnly(2022, 12, 25),
                new DateOnly(2022, 12, 26)
            };
        }

        [Fact]
        public void PolandHolidays_should_contains_new_year_day()
        {
            PolandNonWorkingDays(2022).Holidays.Should().Contain(new DateOnly(2022, 01, 01));
        }

        [Fact]
        public void PolandHolidays_should_contains_easter_day()
        {
            PolandNonWorkingDays(2022).Holidays.Should().Contain(new DateOnly(2022, 04, 17));
        }

        [Fact]
        public void PolandHolidays_should_contains_monday_easter_day()
        {
            PolandNonWorkingDays(2022).Holidays.Should().Contain(expected: new DateOnly(2022, 04, 18));
        }

        [Fact]
        public void PolandHolidays_easter_day_for_2021_should_be_expected()
        {
            PolandNonWorkingDays(2021).Holidays.Should().Contain(expected: new DateOnly(2021, 04, 04));
        }

        [Fact]
        public void PolandHolidays_should_contains_thirteen_days()
        {
            PolandNonWorkingDays(2022).Holidays.Should().HaveCount(13);
        }

        [Fact]
        public void PolandHolidays_should_contains_all_expected_days_for_2022()
        {
            PolandNonWorkingDays(2022).Holidays.ToHashSet().Should().BeEquivalentTo(expectedPolandHolidaysForYear2022);
        }

        [Fact]
        public void PolandHolidays_should_contains_only_unique_dates()
        {
            PolandNonWorkingDays(2022).Holidays.Should().OnlyHaveUniqueItems();
        }
    }
}
