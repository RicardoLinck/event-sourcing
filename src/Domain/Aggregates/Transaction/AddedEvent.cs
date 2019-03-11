using System;
using EventSourcing.Base;

namespace EventSourcing.Domain.Aggregates.Transaction
{
    public class AddedEvent : Event
    {
        public AddedEvent(decimal amount, Card card) 
        {
            Amount = amount;
            Card = card;
        }

        public decimal Amount { get; }
        public Card Card { get; }
    }
}