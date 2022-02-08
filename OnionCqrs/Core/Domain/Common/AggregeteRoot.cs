using System.Collections.Generic;

namespace Domain.Common
{
    public abstract class AggregeteRoot<TId> : Entity<TId>
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

        protected virtual void AddDomainEvent(IDomainEvent newEvent) => _domainEvents.Add(newEvent);

        public virtual void ClearEvents() => _domainEvents.Clear();
    }

    public abstract class AggregateRoot : AggregeteRoot<long> { }
}
