using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryProperty
{
    internal class CategoryPropertyDo : IEntityWithIdDo
    {
        public ObjectId CategoryId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}