using System;
using EventSourcing.Base;

namespace EventSourcing.Domain.Aggregates.Transaction
{
    public class AuthorizedEvent : Event
    {
        public AuthorizedEvent(decimal authorizedAmount)
        {
            AuthorizedAmount = authorizedAmount;
        }
        public decimal AuthorizedAmount { get; }
    }
}