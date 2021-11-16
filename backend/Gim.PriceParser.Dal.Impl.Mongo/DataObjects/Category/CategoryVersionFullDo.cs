using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Category
{
    internal class CategoryVersionFullDo : IEntityWithIdDo
    {
        public CategoryFullDo Entity { get; set; }
        public long SeqId { get; set; }
        public ObjectId Id { get; set; }
    }
}