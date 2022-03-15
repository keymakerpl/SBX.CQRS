using Application.Common;
using CSharpFunctionalExtensions;
using System;

namespace Application.Domain.Projects.Commands.RegisterProject
{
    public class RegisterProjectCommand : ICommand<Result<ProjectDto>>
    {
        public RegisterProjectCommand(string name,
                                      int developersLimit,
                                      int codeLinesToWrite,
                                      DateOnly startDate,
                                      DateOnly deadLine)
        {
            Name = name;
            DevelopersLimit = developersLimit;
            CodeLinesToWrite = codeLinesToWrite;
            StartDate = startDate;
            DeadLine = deadLine;
        }

        public string Name { get; }
        public int CodeLinesToWrite { get; }
        public int DevelopersLimit { get; }
        public DateOnly DeadLine { get; }
        public DateOnly StartDate { get; }
    }
}