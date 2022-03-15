using Application.Common;
using CSharpFunctionalExtensions;

namespace Application.Domain.Projects.Commands.AssignDeveloper
{
    public class AssignDeveloperCommand : ICommand<Result>
    {
        public AssignDeveloperCommand(long projectId, long developerId)
        {
            ProjectId = projectId;
            DeveloperId = developerId;
        }

        public long ProjectId { get; }
        public long DeveloperId { get; }
    }
}
