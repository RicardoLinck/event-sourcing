using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcing.Base;

namespace EventSourcing.Infrastructure.Data
{
    public sealed class EventSourcingRepository<TAggregate> : IEventSourcingRepository<TAggregate> where TAggregate : EventSourcingAggregate
    {
        private readonly IEventSourcingStore _eventSourcingStore;
        public EventSourcingRepository(IEventSourcingStore eventSourcingStore)
        {
            _eventSourcingStore = eventSourcingStore;
        }

        public async Task<TAggregate> LoadAsync(Guid aggregateId)
        {
            var events = await _eventSourcingStore.FindEventsByAggregateIdAsync(aggregateId);
            if (!events.Any())
                throw new DomainException("Aggregate not found.");

            var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), new object[] { aggregateId, events });
            return aggregate;
        }

        public async Task SaveAsync(TAggregate aggregate)
        {
            await _eventSourcingStore.SaveEventsAsync(aggregate.PendingEvents);
            aggregate.PendingEvents.Clear();
        }
    }
}