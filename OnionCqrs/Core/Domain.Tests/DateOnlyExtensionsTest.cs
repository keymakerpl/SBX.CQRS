using Application.Domain.SharedKernel;
using Domain.Extensions;
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
            var actualDay = new DateOnly(2022, 02, 18).AddWorkingDays(2, NonWorkingDays.PolandNonWorkingDays(2022));

            actualDay.Should().BeEquivalentTo(expectedDay);
        }
    }
}
