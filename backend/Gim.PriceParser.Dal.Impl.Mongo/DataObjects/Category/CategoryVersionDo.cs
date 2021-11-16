using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Category
{
    internal class CategoryVersionDo : IEntityVersionDo<CategoryDo>
    {
        public CategoryDo Entity { get; set; }
        public long SeqId { get; set; }
        public ObjectId Id { get; set; }
    }
}