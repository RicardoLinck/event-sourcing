using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourcing.Base
{
    public interface IEventSourcingStore
    {
         Task<IEnumerable<Event>> FindEventsByAggregateIdAsync(Guid aggregateId);
         Task SaveEventsAsync(IEnumerable<Event> events);
    }
}