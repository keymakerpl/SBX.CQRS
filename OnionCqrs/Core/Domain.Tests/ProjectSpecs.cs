using CSharpFunctionalExtensions;
using Domain.Utils;
using Domain.Projects;
using FluentAssertions;
using System;
using Xunit;
using Xunit.Abstractions;
using static Domain.Projects.WorkTime;
using static Domain.Projects.Status;

namespace Domain.Tests
{
    public sealed class ProjectSpecs
    {
        private readonly Developer[] developers;
        private readonly ITestOutputHelper outputHelper;

        public ProjectSpecs(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            this.developers = new Developer[]
            {
                new Developer(PersonName.Create("Radek").Value, PersonName.Create("Kurek").Value, 120, FullTimeWorker),
                new Developer(PersonName.Create("Jan").Value, PersonName.Create("Nowak").Value, 220, FullTimeWorker),
                new Developer(PersonName.Create("Badyl").Value, PersonName.Create("Kotwica").Value, 340, PartTimeWorker),
                new Developer(PersonName.Create("Alina").Value, PersonName.Create("Bolek").Value, 460, FullTimeWorker),
                new Developer(PersonName.Create("Anna").Value, PersonName.Create("Patyk").Value, 520, PartTimeWorker),
                new Developer(PersonName.Create("Bartek").Value, PersonName.Create("Kania").Value, 344, FullTimeWorker),
                new Developer(PersonName.Create("Maria").Value, PersonName.Create("Kaktus").Value, 114, None),
                new Developer(PersonName.Create("Krystian").Value, PersonName.Create("Papka").Value, 0, FullTimeWorker)
            };
        }

        [Fact]
        public void AssignDeveloper_not_exceeded_limit_should_return_success_result()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(startDay, deadline, 5, 5000, NonWorkingDays.PolandNonWorkingDays(startDay.Year));

            project.AssignDeveloper(developers[0]);
            project.AssignDeveloper(developers[1]).Should().Be(Result.Success());
        }

        [Fact]
        public void AssignDeveloper_exceeded_limit_should_return_failure_result()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(startDay, deadline, 2, 5000, NonWorkingDays.PolandNonWorkingDays(startDay.Year));

            project.AssignDeveloper(developers[0]);
            project.AssignDeveloper(developers[1]);
            project.AssignDeveloper(developers[2])
                   .Should()
                   .Be(Result.Failure("Developers limit for this project exceed limit of 2"));
        }

        [Fact]
        public void UnassignDeveloper_should_return_not_found_failure_result()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(startDay, deadline, 5, 5000, NonWorkingDays.PolandNonWorkingDays(startDay.Year));

            project.UnassignDeveloper(developers[0])
                   .Should()
                   .Be(Result.Failure("Cannot remove or not found!"));
        }

        [Fact]
        public void UnassignDeveloper_should_return_success_result()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(startDay, deadline, 5, 5000, NonWorkingDays.PolandNonWorkingDays(startDay.Year));

            project.AssignDeveloper(developers[0]);
            project.UnassignDeveloper(developers[0])
                   .Should()
                   .Be(Result.Success());
        }

        [Fact]
        public void CalculateEstimatedDateOfComplete_should_return_expected_date()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(startDay, deadline, 5, 5000, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);

            project.CalculateEstimatedDateOfComplete()
                   .Should()
                   .Be(new DateOnly(2022, 2, 18));
        }

        [Fact]
        public void CalculateEstimatedDateOfComplete_with_two_developers_should_return_expected_date()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(startDay, deadline, 5, 5000, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);
            project.AssignDeveloper(developers[1]);

            project.CalculateEstimatedDateOfComplete()
                   .Should()
                   .Be(new DateOnly(2022, 2, 14));
        }

        [Fact]
        public void CalculateEstimatedDateOfComplete_three_developers_should_return_expected_date()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(startDay, deadline, 5, 5000, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);
            project.AssignDeveloper(developers[1]);
            project.AssignDeveloper(developers[2]);

            project.CalculateEstimatedDateOfComplete()
                   .Should()
                   .Be(new DateOnly(2022, 2, 14));
        }

        [Fact]
        public void CalculateEstimatedDateOfComplete_four_developers_should_return_expected_date()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(startDay, deadline, 5, 5000, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);
            project.AssignDeveloper(developers[1]);
            project.AssignDeveloper(developers[2]);
            project.AssignDeveloper(developers[3]);

            project.CalculateEstimatedDateOfComplete()
                   .Should()
                   .Be(new DateOnly(2022, 2, 11));
        }

        [Fact]
        public void CalculateEstimatedDateOfComplete_should_return_same_date_as_start_date()
        {
            var startDay = new DateOnly(2022, 2, 08);
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);

            project.CalculateEstimatedDateOfComplete()
                   .Should()
                   .Be(new DateOnly(2022, 2, 8));
        }

        [Fact]
        public void CanApprove_should_return_success_result()
        {
            var startDay = new DateOnly(2022, 02, 22);
            var deadline = new DateOnly(2022, 02, 22);
            var dateOfApprove = new DateOnly(2022, 02, 22);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0])
                   .OnFailure(error => outputHelper.WriteLine(error));

            project.CanApprove(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeTrue();
        }

        [Fact]
        public void CanApprove_start_day_is_past_should_return_failure_result()
        {
            var startDay = new DateOnly(2021, 2, 08);
            var deadline = new DateOnly(2022, 10, 07);
            var dateOfApprove = new DateOnly(2022, 02, 21);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);

            project.CanApprove(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeFalse();
        }

        [Fact]
        public void CanApprove_no_developers_should_return_failure_result()
        {
            var startDay = new DateOnly(2022, 2, 22);
            var deadline = new DateOnly(2022, 10, 07);
            var dateOfApprove = new DateOnly(2022, 02, 21);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));

            project.CanApprove(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeFalse();
        }

        [Fact]
        public void CanApprove_estimated_finish_date_break_deadline_should_return_failure_result()
        {
            var startDay = new DateOnly(2022, 02, 22);
            var deadline = new DateOnly(2022, 02, 24);
            var dateOfApprove = new DateOnly(2022, 02, 22);
            var project = new Project(startDay, deadline, 5, 5000, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0])
                   .OnFailure(error => outputHelper.WriteLine(error));

            project.CanApprove(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeFalse();
        }

        [Fact]
        public void CanAssignDeveloper_should_success()
        {
            var startDay = new DateOnly(2022, 2, 08);
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));

            project.CanAssignDeveloper(developers[0]).IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void CanAssignDeveloper_with_no_working_hours_should_fail()
        {
            var startDay = new DateOnly(2022, 2, 08);
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));

            project.CanAssignDeveloper(developers[6])
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeFalse();
        }

        [Fact]
        public void CanAssignDeveloper_with_zero_code_lines_per_hour_should_fail()
        {
            var startDay = new DateOnly(2022, 2, 08);
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));

            project.CanAssignDeveloper(developers[7])
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeFalse();
        }

        [Fact]
        public void Approve_should_change_status_from_new_to_approved()
        {
            var startDay = new DateOnly(2022, 2, 22);
            var deadline = new DateOnly(2022, 10, 07);
            var dateOfApprove = new DateOnly(2022, 02, 21);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);

            project.Approve(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeTrue();

            project.Status.HasValue.Should().BeTrue();
            project.Status.Should().BeEquivalentTo(Approved);
        }

        [Fact]
        public void Approve_should_change_status_from_new_to_cancelled()
        {
            var startDay = new DateOnly(2022, 2, 22);
            var deadline = new DateOnly(2022, 10, 07);
            var dateOfApprove = new DateOnly(2022, 02, 21);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);

            project.Cancell()
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeTrue();

            project.Status.HasValue.Should().BeTrue();
            project.Status.Should().BeEquivalentTo(Cancelled);
        }

        [Fact]
        public void Cannot_change_status_from_new_to_in_progress()
        {
            var startDay = new DateOnly(2022, 2, 22);
            var deadline = new DateOnly(2022, 10, 07);
            var dateOfApprove = new DateOnly(2022, 02, 21);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);

            project.Status.HasValue.Should().BeTrue();
            project.Status.Should().BeEquivalentTo(New);

            project.Proceed()
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeFalse();
        }

        [Fact]
        public void Cannot_change_status_from_new_to_on_hold()
        {
            var startDay = new DateOnly(2022, 2, 22);
            var deadline = new DateOnly(2022, 10, 07);
            var dateOfApprove = new DateOnly(2022, 02, 21);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);

            project.Status.HasValue.Should().BeTrue();
            project.Status.Should().BeEquivalentTo(New);

            project.Hold()
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeFalse();
        }

        [Fact]
        public void Cannot_change_status_from_new_to_completed()
        {
            var startDay = new DateOnly(2022, 2, 22);
            var deadline = new DateOnly(2022, 10, 07);
            var dateOfApprove = new DateOnly(2022, 02, 21);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);

            project.Status.HasValue.Should().BeTrue();
            project.Status.Should().BeEquivalentTo(New);

            project.Close()
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeFalse();
        }

        [Fact]
        public void Cannot_change_status_from_inProgress_to_approved()
        {
            var startDay = new DateOnly(2022, 2, 22);
            var deadline = new DateOnly(2022, 10, 07);
            var dateOfApprove = new DateOnly(2022, 02, 21);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);

            project.Status.HasValue.Should().BeTrue();
            project.Status.Should().BeEquivalentTo(New);

            project.Approve(dateOfApprove);
            project.Proceed();

            project.Status.HasValue.Should().BeTrue();
            project.Status.Should().BeEquivalentTo(InProgress);

            project.Approve(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeFalse();
        }

        [Fact]
        public void Cannot_approve_already_approved()
        {
            var startDay = new DateOnly(2022, 2, 22);
            var deadline = new DateOnly(2022, 10, 07);
            var dateOfApprove = new DateOnly(2022, 02, 21);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);

            project.Approve(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeTrue();

            project.Status.HasValue.Should().BeTrue();
            project.Status.Should().BeEquivalentTo(Approved);

            project.Approve(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeFalse();
        }

        [Fact]
        public void Cannot_approve_completed()
        {
            var startDay = new DateOnly(2022, 2, 22);
            var deadline = new DateOnly(2022, 10, 07);
            var dateOfApprove = new DateOnly(2022, 02, 21);
            var project = new Project(startDay, deadline, 5, 120, NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.AssignDeveloper(developers[0]);

            project.Approve(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeTrue();

            project.Status.HasValue.Should().BeTrue();
            project.Status.Should().BeEquivalentTo(Approved);

            project.Complete(new DateOnly(2022, 03, 01))
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeTrue();

            project.Status.HasValue.Should().BeTrue();
            project.Status.Should().BeEquivalentTo(Completed);

            project.Approve(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeFalse();
        }
    }
}
