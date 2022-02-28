using CSharpFunctionalExtensions;
using Domain.Common;
using Domain.Extensions;
using Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static Domain.Projects.Status;

namespace Domain.Projects
{
    public class Project : AggregeteRoot<int>
    {
        private readonly int developersLimit;
        private readonly int codeLinesToWrite;
        private readonly INonWorkingDays workingDays;
        private readonly DateOnly startDate;
        private readonly DateOnly deadLine;

        private DevelopersCollection Developers { get; } = new DevelopersCollection();

        private Dictionary<Status, List<(Trigger, Status)>> StateRules { get; } =
            new Dictionary<Status, List<(Trigger, Status)>>()
            {
                { New,
                    new List<(Trigger, Status)>
                    {
                        (Trigger.Approve, Approved),
                        (Trigger.Cancell, Cancelled)
                    }
                },
                { Approved,
                    new List<(Trigger, Status)>
                    {
                        (Trigger.Hold, OnHold),
                        (Trigger.Proceed, InProgress),
                        (Trigger.Cancell, Cancelled),
                        (Trigger.Done, Completed)
                    }
                },
                { InProgress,
                    new List<(Trigger, Status)>
                    {
                        (Trigger.Hold, OnHold),
                        (Trigger.Cancell, Cancelled),
                        (Trigger.Done, Completed)
                    }
                },
                { OnHold,
                    new List<(Trigger, Status)>
                    {
                        (Trigger.Proceed, InProgress),
                        (Trigger.Cancell, Cancelled),
                        (Trigger.Done, Completed)
                    }
                }
            };

        public Project(DateOnly startDate,
                       DateOnly deadLine,
                       int developersLimit,
                       int codeLinesToWrite,
                       INonWorkingDays workingDays)
        {
            this.startDate = startDate;
            this.deadLine = deadLine;
            this.developersLimit = developersLimit;
            this.codeLinesToWrite = codeLinesToWrite;
            this.workingDays = workingDays;
        }

        public Maybe<Status> Status { get; private set; } = New;

        public Result CanAssignDeveloper(Developer developer) =>
            Result.SuccessIf(developer.WorkTime.HoursPerDay > 0, developer, "Developer should work at least for one hour!")
                  .Ensure(developer => developer.CodeLinesPerHour > 0, "Developer can't write any code!")
                  .Bind(_ => Result.SuccessIf(Developers.Count() < this.developersLimit,
                        $"Developers limit for this project exceed limit of {this.developersLimit}"));

        public Result AssignDeveloper(Developer developer) =>
            CanAssignDeveloper(developer)
            .Bind(() => this.Developers.Add(developer));

        public Result UnassignDeveloper(Developer developer) =>
            this.Developers.Remove(developer);

        private Result CanChangeStatus(Trigger trigger) =>
            Result.SuccessIf(StateRules.ContainsKey(Status.Value), $"Cannot {trigger} project when status is {Status}")
                  .Map(() => StateRules[Status.Value].Select(x => x.Item1))
                  .Ensure(triggers => triggers.Contains(trigger), triggers =>
                    $"Cannot {Enum.GetName(trigger)} project when staus is {Status}, allowed actions are " +
                    $"{triggers.Select(x => Enum.GetName(x)).Aggregate((s1, s2) => $"{s1}, {s2}")}");

        public DateOnly CalculateEstimatedDateOfComplete() =>
            Result.Success(Developers.CalculateEstimatedCodeLinesPerDay())
                  .Map(codeLinesPerDay => Math.Ceiling(codeLinesToWrite / codeLinesPerDay))
                  .Map(daysToComplete =>
                        startDate.AddWorkingDays(workingDays, Convert.ToInt32(daysToComplete)))
                  .Finally(dateToReturn => dateToReturn.Value);

        public Result CanApprove(DateOnly dateOfApprove) =>
            Result.SuccessIf(startDate >= dateOfApprove, "Cannot approve project with start date prior to current date!")
                  .Ensure(() => Developers.Any(), "No developers! Assign at least one developer!")
                  .Bind(() => CanChangeStatus(Trigger.Approve))
                  .Map(() => CalculateEstimatedDateOfComplete())
                  .Ensure(estimatedDateOfFinish =>
                          estimatedDateOfFinish <= deadLine,
                          date =>
                            $"Estimated day of finish {date} exceeds day of deadline {deadLine}! Assign more developers!");

        public Result Approve(DateOnly dateOfApprove) =>
            CanApprove(dateOfApprove).Tap(() => Status = Approved);

        public Result Complete(DateOnly dateOfComplete) =>
            CanChangeStatus(Trigger.Done)
            .Ensure(() => dateOfComplete >= startDate, "Date of complete is smaller than start date!")
            .Tap(() => Status = Completed);

        public Result Hold() => CanChangeStatus(Trigger.Hold).Tap(() => Status = OnHold);

        public Result Proceed() => CanChangeStatus(Trigger.Proceed).Tap(() => Status = InProgress);

        public Result Close() => CanChangeStatus(Trigger.Done).Tap(() => Status = Completed);

        public Result Cancell() => CanChangeStatus(Trigger.Cancell).Tap(() => Status = Cancelled);
    }
}
