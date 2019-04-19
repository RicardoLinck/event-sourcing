using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Conventions;

namespace EventSourcing.Infrastructure.Data
{
    public sealed class AssemblyQualifiedNameDiscriminatorConvention : IDiscriminatorConvention
    {
        public string ElementName => "_t";

        public Type GetActualType(IBsonReader bsonReader, Type nominalType)
        {
            var bookmark = bsonReader.GetBookmark();
            bsonReader.ReadStartDocument();
            string typeValue = string.Empty;
            if (bsonReader.FindElement(ElementName))
                typeValue = bsonReader.ReadString();
            else
                throw new NotSupportedException();

            bsonReader.ReturnToBookmark(bookmark);
            return Type.GetType(typeValue);
        }

        public BsonValue GetDiscriminator(Type nominalType, Type actualType)
        {
            return actualType.AssemblyQualifiedName;
        }
    }
}