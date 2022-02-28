using Domain.Extensions;
using Domain.Utils;
using FluentAssertions;
using System;
using Xunit;

namespace Domain.Tests
{
    public class DateOnlyExtensionsTest
    {
        [Fact]
        public void CalculateWorkingDays_should_return_expected_working_days()
        {
            var expectedDay = new DateOnly(2022, 02, 21);
            var actualDay = new DateOnly(2022, 02, 18).AddWorkingDays(NonWorkingDays.PolandNonWorkingDays(2022), 2);

            actualDay.Should().BeEquivalentTo(expectedDay);
        }
    }
}
