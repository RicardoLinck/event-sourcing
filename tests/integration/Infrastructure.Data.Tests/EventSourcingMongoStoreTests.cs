using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcing.Base;
using EventSourcing.Domain.Aggregates.Transaction;
using EventSourcing.Infrastructure.Data;
using MongoDB.Driver;
using NUnit.Framework;

namespace Tests
{
    public class EventSourcingMongoStoreTests
    {
        private EventSourcingMongoStore _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new EventSourcingMongoStore("mongodb://localhost:27017", "transactions", "events");
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("transactions");
            database.DropCollection("events");
            database.CreateCollection("events");
        }

        [Test]
        public async Task SavingEventsInsertsOnTheCollection()
        {
            // Arrange
            var aggregateId = Guid.NewGuid();
            var events = new List<Event>()
            {
                new AddedEvent(10, new Card("12345678912345", new ExpiryDate(2020, 10)))
                {
                    Id = Guid.NewGuid(),
                    AggregateId = aggregateId,
                    Created = DateTime.UtcNow,
                    Version = 1
                },
                new AuthorizedEvent(10)
                {
                    Id = Guid.NewGuid(),
                    AggregateId = aggregateId,
                    Created = DateTime.UtcNow,
                    Version = 2
                },
            };

            // Act
            await _sut.SaveEventsAsync(events);
            var dbEvents = await _sut.FindEventsByAggregateIdAsync(aggregateId);

            // Assert
            Assert.AreEqual(events.Count, dbEvents.Count());
        }
    }
}