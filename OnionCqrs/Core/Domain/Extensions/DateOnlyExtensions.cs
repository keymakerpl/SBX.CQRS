using Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Extensions
{
    public static class DateOnlyExtensions
    {
        public static DateOnly AddWorkingDays(this DateOnly dateFrom,
                                              int days,
                                              INonWorkingDays nonWorkingDays)
        {
            var dateToCheck = new DateOnly(dateFrom.Year, dateFrom.Month, dateFrom.Day);
            var workingDays = new List<DateOnly>();
            while (days > 0)
            {
                if (nonWorkingDays.ContainsDay(dateToCheck).IsSuccess)
                {
                    days--;
                    workingDays.Add(dateToCheck);
                }
                dateToCheck = dateToCheck.AddDays(1);
            }

            return workingDays.Max();
        }
    }
}
