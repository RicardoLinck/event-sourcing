using System;
using System.Collections.Generic;
using System.Linq;
using EventSourcing.Base;
using EventSourcing.Domain.Aggregates.Transaction;
using NUnit.Framework;

namespace Tests
{
    public class TransactionAggregateTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void NewSutWithNullEventHistoryThrowsException()
        {
            // Arrange & Act & Assert
            Assert.That(() => new TransactionAggregate(Guid.NewGuid(), null), Throws.ArgumentNullException);
        }

        [Test]
        public void NewSutRaisesAddedEvent()
        {
            // Arrange & Act
            var sut = new TransactionAggregate(Guid.NewGuid(), 10.99M, new Card("1234567890", new ExpiryDate(2020, 10)));
            
            // Assert
            Assert.That(sut.UncommittedEvents.First(), Is.TypeOf<AddedEvent>());
        }

        [Test]
        public void AuthorizingRaisesAuthorizedEvent()
        {
            // Arrange
            var amount = 10.99M;
            var sut = new TransactionAggregate(Guid.NewGuid(), amount, new Card("1234567890", new ExpiryDate(2020, 10)));

            // Act
            sut.UncommittedEvents.Clear();
            sut.Authorize(amount);

            // Assert
            AuthorizedEvent authorizedEvent;
            Assert.That(authorizedEvent = (AuthorizedEvent)sut.UncommittedEvents.First(), Is.TypeOf<AuthorizedEvent>());
            Assert.AreEqual(amount, authorizedEvent.AuthorizedAmount);
            Assert.AreEqual(amount, sut.AuthorizedAmount);
        }

        [Test]
        public void NewSutFromEventHistoryAppliesEvents()
        {
            // Arrange
            var amount = 10.99M;
            var card = new Card("1234567890", new ExpiryDate(2020, 10));
            var id = Guid.NewGuid();
            var eventHistory = new List<Event>()
            {
                {new AddedEvent(amount,card){Version = 1, Id = Guid.NewGuid(), SourceId = id}},
                {new AuthorizedEvent(amount){Version = 2, Id = Guid.NewGuid(), SourceId = id}}
            };

            // Act
            var sut = new TransactionAggregate(id, eventHistory);

            // Assert
            Assert.AreEqual(amount, sut.Amount);
            Assert.AreEqual(card, sut.Card);
            Assert.AreEqual(amount, sut.AuthorizedAmount);
        }
    }
}