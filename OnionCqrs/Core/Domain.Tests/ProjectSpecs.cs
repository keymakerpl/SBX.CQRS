using CSharpFunctionalExtensions;
using Domain.Projects;
using FluentAssertions;
using System;
using Xunit;
using Xunit.Abstractions;
using static Domain.Projects.WorkTime;
using static Domain.Projects.Status;
using Domain.SharedKernel;
using Application.Domain.SharedKernel;

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
                Developer.CreateDeveloper(PersonName.Create("Radek").Value, PersonName.Create("Kurek").Value, 120, FullTimeWorker).Value,
                Developer.CreateDeveloper(PersonName.Create("Jan").Value, PersonName.Create("Nowak").Value, 220, FullTimeWorker).Value,
                Developer.CreateDeveloper(PersonName.Create("Badyl").Value, PersonName.Create("Kotwica").Value, 340, PartTimeWorker).Value,
                Developer.CreateDeveloper(PersonName.Create("Alina").Value, PersonName.Create("Bolek").Value, 460, FullTimeWorker).Value,
                Developer.CreateDeveloper(PersonName.Create("Anna").Value, PersonName.Create("Patyk").Value, 520, PartTimeWorker).Value,
                Developer.CreateDeveloper(PersonName.Create("Bartek").Value, PersonName.Create("Kania").Value, 344, FullTimeWorker).Value
            };
        }

        [Fact]
        public void AssignDeveloper_not_exceeded_limit_should_return_success_result()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = Project.Create("Test", startDay, deadline, 5, 5000);
            project.OnFailure(error => outputHelper.WriteLine(error)).IsSuccess.Should().BeTrue();

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);
            project.Value.AssignDeveloper(developers[1]).Should().Be(Result.Success());
        }

        [Fact]
        public void AssignDeveloper_exceeded_limit_should_return_failure_result()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = Project.Create("Test", startDay, deadline, 2, 5000);
            project.IsSuccess.Should().BeTrue();

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);
            project.Value.AssignDeveloper(developers[1]);
            project.Value.AssignDeveloper(developers[2])
                   .Should()
                   .Be(Result.Failure("Developers limit for this project exceed limit of 2"));
        }

        [Fact]
        public void UnassignDeveloper_should_return_not_found_failure_result()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = Project.Create("Test", startDay, deadline, 5, 5000);
            project.IsSuccess.Should().BeTrue();

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.UnassignDeveloper(developers[0])
                         .OnFailure(error => outputHelper.WriteLine(error))
                         .IsSuccess
                         .Should()
                         .BeFalse();
        }

        [Fact]
        public void UnassignDeveloper_should_return_success_result()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = Project.Create("Test", startDay, deadline, 5, 5000);
            project.IsSuccess.Should().BeTrue();

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);
            project.Value.Developers.Should().HaveCount(1);
            project.Value.UnassignDeveloper(developers[0])
                         .Should()
                         .Be(Result.Success());

            project.Value.Developers.Should().BeEmpty();
        }

        [Fact]
        public void CalculateEstimatedDateOfComplete_should_return_expected_date()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = Project.Create("Test", startDay, deadline, 5, 5000);
            project.IsSuccess.Should().BeTrue();
            project.Value.AssignDeveloper(developers[0]);

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.CalculateEstimatedDateOfComplete()
                   .Should()
                   .Be(new DateOnly(2022, 2, 18));
        }

        [Fact]
        public void CalculateEstimatedDateOfComplete_with_two_developers_should_return_expected_date()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = Project.Create("Test", startDay, deadline, 5, 5000);
            project.IsSuccess.Should().BeTrue();

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);
            project.Value.AssignDeveloper(developers[1]);

            project.Value.CalculateEstimatedDateOfComplete()
                   .Should()
                   .Be(new DateOnly(2022, 2, 14));
        }

        [Fact]
        public void CalculateEstimatedDateOfComplete_three_developers_should_return_expected_date()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = Project.Create("Test", startDay, deadline, 5, 5000);
            project.IsSuccess.Should().BeTrue();

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);
            project.Value.AssignDeveloper(developers[1]);
            project.Value.AssignDeveloper(developers[2]);

            project.Value.CalculateEstimatedDateOfComplete()
                   .Should()
                   .Be(new DateOnly(2022, 2, 14));
        }

        [Fact]
        public void CalculateEstimatedDateOfComplete_four_developers_should_return_expected_date()
        {
            var startDay = new DateOnly(2022, 2, 11);
            var deadline = new DateOnly(2022, 10, 07);
            var project = Project.Create("Test", startDay, deadline, 5, 5000);
            project.IsSuccess.Should().BeTrue();

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);
            project.Value.AssignDeveloper(developers[1]);
            project.Value.AssignDeveloper(developers[2]);
            project.Value.AssignDeveloper(developers[3]);

            project.Value.CalculateEstimatedDateOfComplete()
                   .Should()
                   .Be(new DateOnly(2022, 2, 11));
        }

        [Fact]
        public void CalculateEstimatedDateOfComplete_should_return_same_date_as_start_date()
        {
            var startDay = new DateOnly(2022, 2, 08);
            var deadline = new DateOnly(2022, 10, 07);
            var project = Project.Create("Test", startDay, deadline, 5, 120);
            project.IsSuccess.Should().BeTrue();
            project.Value.AssignDeveloper(developers[0]);

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.CalculateEstimatedDateOfComplete()
                   .Should()
                   .Be(new DateOnly(2022, 2, 8));
        }

        [Fact]
        public void CanApprove_should_return_success_result()
        {
            var startDay = new DateOnly(2022, 02, 22);
            var deadline = new DateOnly(2022, 02, 22);
            var dateOfApprove = new DateOnly(2022, 02, 22);
            var project = Project.Create("Test", startDay, deadline, 5, 120);
            project.OnFailure(error => outputHelper.WriteLine(error)).IsSuccess.Should().BeTrue();
            project.Value.AssignDeveloper(developers[0])
                   .OnFailure(error => outputHelper.WriteLine(error));

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.CanApprove(dateOfApprove)
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
            var project = Project.Create("Test", startDay, deadline, 5, 120);
            project.IsSuccess.Should().BeTrue();
            project.Value.AssignDeveloper(developers[0]);

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.CanApprove(dateOfApprove)
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
            var project = Project.Create("Test", startDay, deadline, 5, 120);
            project.IsSuccess.Should().BeTrue();

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.CanApprove(dateOfApprove)
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
            var project = Project.Create("Test", startDay, deadline, 5, 5000);
            project.IsSuccess.Should().BeTrue();

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0])
                   .OnFailure(error => outputHelper.WriteLine(error));

            project.Value.CanApprove(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeFalse();
        }

        [Fact]
        public void CanAssignDeveloper_should_success()
        {
            var startDay = new DateOnly(2022, 2, 08);
            var deadline = new DateOnly(2022, 10, 07);
            var project = Project.Create("Test", startDay, deadline, 5, 120);
            project.IsSuccess.Should().BeTrue();

            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.CanAssignDeveloper(developers[0]).IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Approve_should_change_status_from_new_to_approved()
        {
            var startDay = new DateOnly(2022, 2, 22);
            var deadline = new DateOnly(2022, 10, 07);
            var dateOfApprove = new DateOnly(2022, 02, 21);
            var project = Project.Create("Test", startDay, deadline, 5, 120);
            project.IsSuccess.Should().BeTrue();
            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);

            project.Value.Approve(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeTrue();

            project.Value.Status.Should().NotBeNull();
            project.Value.Status.Should().BeEquivalentTo(Approved);
        }

        [Fact]
        public void Approve_should_change_status_from_new_to_cancelled()
        {
            var startDay = new DateOnly(2022, 2, 22);
            var deadline = new DateOnly(2022, 10, 07);
            var dateOfApprove = new DateOnly(2022, 02, 21);
            var project = Project.Create("Test", startDay, deadline, 5, 120);
            project.IsSuccess.Should().BeTrue();
            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);

            project.Value.Cancell()
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeTrue();

            project.Value.Status.Should().NotBeNull();
            project.Value.Status.Should().BeEquivalentTo(Cancelled);
        }

        [Fact]
        public void Cannot_change_status_from_new_to_in_progress()
        {
            var startDay = new DateOnly(2022, 2, 22);
            var deadline = new DateOnly(2022, 10, 07);
            var dateOfApprove = new DateOnly(2022, 02, 21);
            var project = Project.Create("Test", startDay, deadline, 5, 120);
            project.IsSuccess.Should().BeTrue();
            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);

            project.Value.Status.Should().NotBeNull();
            project.Value.Status.Should().BeEquivalentTo(New);

            project.Value.Proceed()
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
            var project = Project.Create("Test", startDay, deadline, 5, 120);
            project.IsSuccess.Should().BeTrue();
            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);

            project.Value.Status.Should().NotBeNull();
            project.Value.Status.Should().BeEquivalentTo(New);

            project.Value.Hold()
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
            var project = Project.Create("Test", startDay, deadline, 5, 120);
            project.IsSuccess.Should().BeTrue();
            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);

            project.Value.Status.Should().NotBeNull();
            project.Value.Status.Should().BeEquivalentTo(New);

            project.Value.Close()
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
            var project = Project.Create("Test", startDay, deadline, 5, 120);
            project.IsSuccess.Should().BeTrue();
            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);

            project.Value.Status.Should().NotBeNull();
            project.Value.Status.Should().BeEquivalentTo(New);

            project.Value.Approve(dateOfApprove);
            project.Value.Proceed();

            project.Value.Status.Should().NotBeNull();
            project.Value.Status.Should().BeEquivalentTo(InProgress);

            project.Value.Approve(dateOfApprove)
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
            var project = Project.Create("Test", startDay, deadline, 5, 120);
            project.IsSuccess.Should().BeTrue();
            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);

            project.Value.Approve(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeTrue();

            project.Value.Status.Should().NotBeNull();
            project.Value.Status.Should().BeEquivalentTo(Approved);

            project.Value.Approve(dateOfApprove)
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
            var project = Project.Create("Test", startDay, deadline, 5, 120);
            project.IsSuccess.Should().BeTrue();
            project.Value.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(startDay.Year));
            project.Value.AssignDeveloper(developers[0]);

            project.Value.Approve(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeTrue();

            project.Value.Status.Should().NotBeNull();
            project.Value.Status.Should().BeEquivalentTo(Approved);

            project.Value.Complete(new DateOnly(2022, 03, 01))
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeTrue();

            project.Value.Status.Should().NotBeNull();
            project.Value.Status.Should().BeEquivalentTo(Completed);

            project.Value.Approve(dateOfApprove)
                   .OnFailure(error => outputHelper.WriteLine(error)).IsSuccess
                   .Should()
                   .BeFalse();
        }
    }
}
