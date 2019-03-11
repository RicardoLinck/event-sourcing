using System;
using System.Collections.Generic;

namespace EventSourcing.Base
{
    public class EventSourcingRepository<TAggregate> where TAggregate : EventSourcingAggregate
    {
        public void LoadHistory(Guid id)
        {
            // ToDo: get history from store
            var list = new List<Event>();
            
            Activator.CreateInstance(typeof(TAggregate), new object[]{id, list});
        }
    }
}