using System;

namespace EventSourcing.Base
{
    public class Event
    {
        public Guid Id { get; set;}
        public Guid AggregateId { get; set;}
        public int Version { get; set;}
        public DateTime Created { get; set;}
    }
}