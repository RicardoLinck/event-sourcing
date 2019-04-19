using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcing.Base;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace EventSourcing.Infrastructure.Data
{
    public class EventSourcingMongoStore: IEventSourcingStore
    {
        private readonly IMongoCollection<Event> _collection;

        public EventSourcingMongoStore(string connectionString, string database, string collectionName)
        {
            var client = new MongoClient(connectionString);
            BsonClassMap.LookupClassMap(typeof(Event));
            BsonSerializer.RegisterDiscriminatorConvention(typeof(Event), new AssemblyQualifiedNameDiscriminatorConvention());
            _collection = client.GetDatabase(database).GetCollection<Event>(collectionName);
        }

        public async Task<IEnumerable<Event>> FindEventsByAggregateIdAsync(Guid aggregateId)
        {
            return await _collection
                .Find(e => e.AggregateId == aggregateId)
                .SortBy(e => e.Version)
                .ToListAsync();
        }

        public Task SaveEventsAsync(IEnumerable<Event> events)
        {
            return _collection.InsertManyAsync(events.OrderBy(e => e.Version));
        }
    }
}