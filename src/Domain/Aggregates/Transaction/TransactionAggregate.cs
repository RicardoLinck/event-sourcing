using System;
using System.Collections.Generic;
using EventSourcing.Base;

namespace EventSourcing.Domain.Aggregates.Transaction
{
    public sealed class TransactionAggregate : EventSourcingAggregate
    {
        public decimal Amount { get; private set; }
        public decimal AuthorizedAmount { get; private set; }
        public Card Card { get; private set; }

        public TransactionAggregate(Guid id, IEnumerable<Event> eventHistory) : base(id, eventHistory)
        {
            HandleEvents();
            LoadFromHistory(eventHistory);
        }

        private TransactionAggregate(Guid id) : base(id)
        {
            HandleEvents();
        }

        private void HandleEvents()
        {
            Handles<AddedEvent>(Apply);
            Handles<AuthorizedEvent>(Apply);
        }

        public TransactionAggregate(Guid id, decimal amount, Card card) : this(id)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            if (amount == 0.0M)
            {
                throw new ArgumentException("INVALID_AMOUNT", nameof(card));
            }

            HandleEvent(new AddedEvent(amount, card));
        }

        public void Authorize(decimal authorizedAmount)
        {
            HandleEvent(new AuthorizedEvent(authorizedAmount));
        }

        private void Apply(AddedEvent addedEvent)
        {
            Amount = addedEvent.Amount;
            Card = addedEvent.Card;
        }

        private void Apply(AuthorizedEvent authorizedEvent)
        {
            AuthorizedAmount = authorizedEvent.AuthorizedAmount;
        }
    }
}