using Application.Common;
using CSharpFunctionalExtensions;

namespace Application.Domain.Projects.Commands.ChangeStatus
{
    public class ChangeStatusCommand : ICommand<Result>
    {
        public long ProjectId { get; }
        public Status Status { get; }

        public ChangeStatusCommand(long projectId, Status status)
        {
            ProjectId = projectId;
            Status = status;
        }
    }
}
