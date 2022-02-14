using FluentAssertions;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using static Domain.Utils.Holidays;

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
            PolandHolidays(2022).All.Should().Contain(new DateOnly(2022, 01, 01));
        }

        [Fact]
        public void PolandHolidays_should_contains_easter_day()
        {
            PolandHolidays(2022).All.Should().Contain(new DateOnly(2022, 04, 17));
        }

        [Fact]
        public void PolandHolidays_should_contains_monday_easter_day()
        {
            PolandHolidays(2022).All.Should().Contain(expected: new DateOnly(2022, 04, 18));
        }

        [Fact]
        public void PolandHolidays_should_contains_thirteen_days()
        {
            PolandHolidays(2022).All.Should().HaveCount(13);
        }

        [Fact]
        public void PolandHolidays_should_contains_all_expected_days_for_2022()
        {
            PolandHolidays(2022).All.ToHashSet().Should().BeEquivalentTo(expectedPolandHolidaysForYear2022);
        }

        [Fact]
        public void PolandHolidays_should_contains_only_unique_dates()
        {
            PolandHolidays(2022).All.Should().OnlyHaveUniqueItems();
        }
    }
}
