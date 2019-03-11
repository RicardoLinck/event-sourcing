using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSourcing.Base
{
    public class EventSourcingAggregate
    {
        private readonly Dictionary<Type, Action<Event>> _handlers = new Dictionary<Type, Action<Event>>();

        public List<Event> UncommittedEvents { get; } = new List<Event>();
        public int Version { get; private set; }
        public Guid Id { get; }

        protected EventSourcingAggregate(Guid id)
        {
            Id = id;
        }

        protected EventSourcingAggregate(Guid id, IEnumerable<Event> eventHistory) : this(id)
        {
            if (eventHistory == null)
            {
                throw new ArgumentNullException(nameof(eventHistory));
            }

            if(!eventHistory.Any())
            {
                throw new ArgumentException("EMPTY_HISTORY",nameof(eventHistory));
            }
        }

        protected void Handles<TEvent>(Action<TEvent> handler) where TEvent : Event
        {
            _handlers[typeof(TEvent)] = (@event) => handler((TEvent)@event);
        }

        protected void HandleEvent(Event @event)
        {
            @event.Id = Guid.NewGuid();
            @event.SourceId = Id;
            @event.Created = DateTime.UtcNow;
            @event.Version = Version + 1;
            _handlers[@event.GetType()].Invoke(@event);
            Version = @event.Version;
            UncommittedEvents.Add(@event);
        }

        protected void LoadFromHistory(IEnumerable<Event> eventHistory)
        {
            foreach (var @event in eventHistory)
            {
                _handlers[@event.GetType()].Invoke(@event);
                Version = @event.Version;
            }
        }
    }
}