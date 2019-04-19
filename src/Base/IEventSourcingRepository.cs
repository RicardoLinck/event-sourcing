using System;
using System.Threading.Tasks;

namespace EventSourcing.Base
{
    public interface IEventSourcingRepository<TAggregate> where TAggregate : EventSourcingAggregate
    {
         Task<TAggregate> LoadAsync(Guid aggregateId);
         Task SaveAsync(TAggregate aggregate);
    }
}