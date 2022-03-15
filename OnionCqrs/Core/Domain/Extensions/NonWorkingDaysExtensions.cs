using CSharpFunctionalExtensions;
using Domain.SharedKernel;
using System;
using System.Linq;

namespace Domain.Extensions
{
    public static class NonWorkingDaysExtensions
    {
        public static Result ContainsDay(this INonWorkingDays nonWorkingDays, DateOnly date) =>
            Result.Combine(
                Result.SuccessIf(nonWorkingDays.WeekDays.Contains(date.DayOfWeek) is false, $"{date} is non working day!"),
                Result.SuccessIf(nonWorkingDays.Holidays.Contains(date) is false, $"{date} is non working day!"));
    }
}
