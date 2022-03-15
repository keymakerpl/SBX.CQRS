using System;
using System.Collections.Generic;

namespace Domain.SharedKernel
{
    public interface INonWorkingDays
    {
        IReadOnlyCollection<DateOnly> Holidays { get; }
        IReadOnlyCollection<DayOfWeek> WeekDays { get; }
    }
}
