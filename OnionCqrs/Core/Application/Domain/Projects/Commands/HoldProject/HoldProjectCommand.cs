using Application.Common;
using CSharpFunctionalExtensions;

namespace Application.Domain.Projects.Commands.HoldProject
{
    public class HoldProjectCommand : ICommand<Result>
    {
        public long ProjectId { get; set; }

        public HoldProjectCommand(long projectId)
        {
            ProjectId = projectId;
        }
    }
}
