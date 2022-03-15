using Application.Common;
using CSharpFunctionalExtensions;

namespace Application.Domain.Projects.Commands.UnassignDeveloper
{
    public class UnassignDeveloperCommand : ICommand<Result>
    {
        public UnassignDeveloperCommand(long projectId, long developerId)
        {
            ProjectId = projectId;
            DeveloperId = developerId;
        }

        public long ProjectId { get; }
        public long DeveloperId { get; }
    }
}
