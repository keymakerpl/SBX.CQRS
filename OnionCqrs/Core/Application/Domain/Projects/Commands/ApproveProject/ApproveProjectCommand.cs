using Application.Common;
using CSharpFunctionalExtensions;

namespace Application.Domain.Projects.Commands.ApproveProject
{
    public class ApproveProjectCommand : ICommand<Result>
    {
        public ApproveProjectCommand(long projectId)
        {
            ProjectId = projectId;
        }

        public long ProjectId { get; }
    }
}
