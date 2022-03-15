using CSharpFunctionalExtensions;
using Domain.Common;
using Domain.Extensions;
using Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using static Domain.Projects.Status;

namespace Domain.Projects
{
    public class Project : AggregateRoot
    {
        private INonWorkingDays nonWorkingDays;

        private Project(string name,
                        DateOnly startDate,
                        DateOnly deadLine,
                        int developersLimit,
                        int codeLinesToWrite)
        {
            this.Name = name;
            this.StartDate = startDate.ToDateTime(TimeOnly.MinValue);
            this.DeadLine = deadLine.ToDateTime(TimeOnly.MinValue);
            this.DevelopersLimit = developersLimit;
            this.CodeLinesToWrite = codeLinesToWrite;
            this.Status = New;
        }

        protected Project() { }

        public static Result<Project> Create(string name,
                                             DateOnly startDate,
                                             DateOnly deadLine,
                                             int developersLimit,
                                             int codeLinesToWrite) => Result.Combine(
            Result.SuccessIf(string.IsNullOrWhiteSpace(name) is false, "Name cannot be empty!"),
            Result.SuccessIf(startDate >= DateOnly.MinValue, $"Start date should be greater than {DateOnly.MinValue}"),
            Result.SuccessIf(startDate <= DateOnly.MaxValue, $"Start date is greater than {DateOnly.MaxValue}"),
            Result.SuccessIf(deadLine >= DateOnly.MinValue, $"Deadline should be greater than {DateOnly.MinValue}"),
            Result.SuccessIf(deadLine <= DateOnly.MaxValue, $"Deadline is greater than {DateOnly.MaxValue}"),
            Result.SuccessIf(startDate <= deadLine, $"Start date is greater than deadline {deadLine}"))
                  .Map(() => new Project(name, startDate, deadLine, developersLimit, codeLinesToWrite));

        public virtual string Name { get; }
        public virtual int CodeLinesToWrite { get; }
        public virtual int DevelopersLimit { get; }
        public virtual DateTime StartDate { get; }
        public virtual DateTime DeadLine { get; }
        public virtual Status Status { get; protected set; } = New;

        public virtual IList<Developer> Developers { get; protected set; } = new List<Developer>();

        private Dictionary<Status, List<(Trigger, Status)>> StatusRules { get; } =
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

        public virtual Result SetNonWorkingDays(INonWorkingDays nonWorkingDays) =>
            Maybe.From(nonWorkingDays).ToResult($"{nameof(nonWorkingDays)} is null!")
                 .Tap(nonWorkingDays => this.nonWorkingDays = nonWorkingDays);

        public virtual Result AssignDeveloper(Developer developer) =>
            CanAssignDeveloper(developer)
            .Tap(() => Developers.AddDeveloper(developer));

        public virtual Result UnassignDeveloper(Developer developer) =>
                 Developers.RemoveDeveloper(developer);

        public virtual Result CanAssignDeveloper(Developer developer) =>
            Result.SuccessIf(developer.WorkTime.HoursPerDay > 0, developer, "Developer should work at least for one hour!")
                  .Ensure(developer => developer.CodeLinesPerHour > 0, "Developer can't write any code!")
                  .Bind(_ => Result.SuccessIf(Developers.Count() < this.DevelopersLimit,
                        $"Developers limit for this project exceed limit of {this.DevelopersLimit}"));

        public virtual DateOnly CalculateEstimatedDateOfComplete() =>
            Result.Try(() => Developers.CalculateEstimatedCodeLinesPerDay())
                  .Map(codeLinesPerDay => Math.Ceiling(CodeLinesToWrite / codeLinesPerDay))
                  .Ensure(_ => nonWorkingDays != null, "Set non working days!")
                  .Map(daysToComplete => DateOnly.FromDateTime(StartDate)
                            .AddWorkingDays(Convert.ToInt32(daysToComplete), nonWorkingDays))
                  .Finally(dateToReturn => dateToReturn.Value);

        public virtual Result Approve(DateOnly dateOfApprove) =>
            CanApprove(dateOfApprove).Tap(() => Status = Approved);

        public virtual Result CanApprove(DateOnly dateOfApprove) =>
            Result.SuccessIf(DateOnly.FromDateTime(StartDate) >= dateOfApprove, "Cannot approve project with start date prior to current date!")
                  .Ensure(() => Developers.Count > 0, "No developers! Assign at least one developer!")
                  .Bind(() => CanChangeStatus(Trigger.Approve))
                  .Map(() => CalculateEstimatedDateOfComplete())
                  .Ensure(estimatedDateOfFinish =>
                          estimatedDateOfFinish <= DateOnly.FromDateTime(DeadLine),
                          date =>
                            $"Estimated day of finish {date} exceeds day of deadline {DeadLine}! Assign more developers!");

        public virtual Result Cancell() => CanChangeStatus(Trigger.Cancell).Tap(() => Status = Cancelled);

        public virtual Result Close() => CanChangeStatus(Trigger.Done).Tap(() => Status = Completed);

        public virtual Result Complete(DateOnly dateOfComplete) =>
            CanChangeStatus(Trigger.Done)
            .Ensure(() => dateOfComplete >= DateOnly.FromDateTime(StartDate), "Date of complete is smaller than start date!")
            .Tap(() => Status = Completed);

        public virtual Result Hold() => CanChangeStatus(Trigger.Hold).Tap(() => Status = OnHold);

        public virtual Result Proceed() => CanChangeStatus(Trigger.Proceed).Tap(() => Status = InProgress);

        private Result CanChangeStatus(Trigger trigger) =>
            Result.SuccessIf(StatusRules.ContainsKey(Status), $"Cannot {trigger} project when status is {Status}")
                  .Map(() => StatusRules[Status].Select(x => x.Item1))
                  .Ensure(triggers => triggers.Contains(trigger), triggers =>
                    $"Cannot {Enum.GetName(trigger)} project when staus is {Status}, allowed actions are " +
                    $"{triggers.Select(x => Enum.GetName(x)).Aggregate((s1, s2) => $"{s1}, {s2}")}");
    }
}