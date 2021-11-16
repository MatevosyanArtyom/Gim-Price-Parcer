using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryPropertyValue
{
    internal class CategoryPropertyValueDo : IEntityWithIdDo
    {
        public ObjectId PropertyId { get; set; }
        public string Name { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}