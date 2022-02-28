using Domain.Common;
using System.Collections.Generic;

namespace Domain.Projects
{
    public enum Trigger
    {
        Approve,
        Hold,
        Proceed,
        Cancell,
        Done
    }

    public sealed class Status : ValueObject
    {
        public static Status New => new("New");
        public static Status Approved => new("Approved");
        public static Status InProgress => new("InProgress");
        public static Status OnHold => new("OnHold");
        public static Status Cancelled => new("Cancelled");
        public static Status Completed => new("Completed");

        private string Value { get; }

        private Status(string status) => this.Value = status;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}
