namespace Application.Domain.Projects.Commands.ChangeStatus
{
    public enum Status
    {
        Approve,
        Hold,
        Proceed,
        Cancell,
        Done
    }

    public class ChangeStatusRequest
    {
        public Status Status { get; set; }
    }
}
