using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventSourcing.Base;
using EventSourcing.Domain.Aggregates.Transaction;
using EventSourcing.Infrastructure.Data;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
    public class EventSourcingRepositoryTests
    {
        private IEventSourcingStore _eventSourcingStore;
        private EventSourcingRepository<TransactionAggregate> _sut;

        [SetUp]
        public void Setup()
        {
            _eventSourcingStore = Substitute.For<IEventSourcingStore>();
            _sut = new EventSourcingRepository<TransactionAggregate>(_eventSourcingStore);
        }

        [Test]
        public void LoadingEmptyHistoryThrowsDomainException()
        {
            // Arrange
            _eventSourcingStore.FindEventsByAggregateIdAsync(Arg.Any<Guid>()).Returns(new List<Event>());

            // Act & Assert
            Assert.That(() => _sut.LoadAsync(Guid.NewGuid()), Throws.TypeOf(typeof(DomainException)));
        }

        [Test]
        public async Task LoadingCallsEventSourcingStore()
        {
            // Arrange
            var aggregateId = Guid.NewGuid();
            _eventSourcingStore.FindEventsByAggregateIdAsync(aggregateId).Returns(
                new List<Event>()
                {
                    new AddedEvent(10, new Card("12345678912345", new ExpiryDate(2020, 10)))
                    {
                        Id = Guid.NewGuid(),
                        AggregateId = aggregateId,
                        Created = DateTime.UtcNow,
                        Version = 1
                    }
                });

            // Act 
            var result = await _sut.LoadAsync(aggregateId);

            // Assert
            await _eventSourcingStore.Received().FindEventsByAggregateIdAsync(aggregateId);
        }

        [Test]
        public async Task LoadingReturnCorrectAggregateType()
        {
            // Arrange
            var aggregateId = Guid.NewGuid();
            _eventSourcingStore.FindEventsByAggregateIdAsync(aggregateId).Returns(
                new List<Event>()
                {
                    new AddedEvent(10, new Card("12345678912345", new ExpiryDate(2020, 10)))
                    {
                        Id = Guid.NewGuid(),
                        AggregateId = aggregateId,
                        Created = DateTime.UtcNow,
                        Version = 1
                    }
                });

            // Act 
            var result = await _sut.LoadAsync(aggregateId);
            
            // Assert
            Assert.IsInstanceOf(typeof(TransactionAggregate), result);
        }
    }
}