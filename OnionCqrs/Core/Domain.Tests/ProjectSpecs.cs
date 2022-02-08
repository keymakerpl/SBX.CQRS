using CSharpFunctionalExtensions;
using Domain.Persons;
using Domain.Persons.Employees.Developers;
using Domain.Projects;
using FluentAssertions;
using System;
using Xunit;
using static Domain.Persons.Employees.Developers.WorkTime;

namespace Domain.Tests
{
    public sealed class ProjectSpecs
    {
        private readonly Developer[] developers;

        public ProjectSpecs()
        {
            this.developers = new Developer[]
            {
                new Developer(PersonName.Create("Radek").Value, PersonName.Create("Kurek").Value,120, FullTimeWorker),
                new Developer(PersonName.Create("Jan").Value, PersonName.Create("Nowak").Value,220, FullTimeWorker),
                new Developer(PersonName.Create("Badyl").Value, PersonName.Create("Kotwica").Value,340, PartTimeWorker)
            };
        }

        [Fact]
        public void GetProjectDevelopersCount_should_return_zero()
        {
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(deadline, 2, 5000);

            project.GetProjectDevelopersCount().Should().Be(0);
        }

        [Fact]
        public void GetProjectDevelopersCount_should_return_one()
        {
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(deadline, 2, 5000);

            project.AssignDeveloper(developers[0]);

            project.GetProjectDevelopersCount().Should().Be(1);
        }

        [Fact]
        public void AssignDeveloper_not_exceeded_limit_should_return_success_result()
        {
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(deadline, 2, 5000);

            project.AssignDeveloper(developers[0]);
            project.AssignDeveloper(developers[1]).Should().Be(Result.Success());
        }

        [Fact]
        public void AssignDeveloper_exceeded_limit_should_return_failure_result()
        {
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(deadline, 2, 5000);

            project.AssignDeveloper(developers[0]);
            project.AssignDeveloper(developers[1]);
            project.AssignDeveloper(developers[2])
                   .Should()
                   .Be(Result.Failure("Developers limit for this project exceed limit of 2"));
        }

        [Fact]
        public void UnassignDeveloper_should_return_not_found_failure_result()
        {
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(deadline, 2, 5000);

            project.UnassignDeveloper(developers[0])
                   .Should()
                   .Be(Result.Failure("Cannot remove or not found!"));
        }

        [Fact]
        public void UnassignDeveloper_should_return_success_result()
        {
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(deadline, 2, 5000);

            project.AssignDeveloper(developers[0]);
            project.UnassignDeveloper(developers[0])
                   .Should()
                   .Be(Result.Success());
        }

        [Fact]
        public void CalculateEstimatedDateOfComplete_should_return_expected_date()
        {
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(deadline, 2, 5000);
            project.AssignDeveloper(developers[0]);

            project.CalculateEstimatedDateOfComplete(new DateOnly(2022, 2, 8))
                   .Should()
                   .Be(new DateOnly(2022, 2, 9));
        }

        [Fact]
        public void CalculateEstimatedDateOfComplete_should_return_same_date_as_start_date()
        {
            var deadline = new DateOnly(2022, 10, 07);
            var project = new Project(deadline, 2, 300);
            project.AssignDeveloper(developers[0]);

            project.CalculateEstimatedDateOfComplete(new DateOnly(2022, 2, 8))
                   .Should()
                   .Be(new DateOnly(2022, 2, 8));
        }
    }
}
