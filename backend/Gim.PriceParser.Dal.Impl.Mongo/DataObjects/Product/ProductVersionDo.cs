using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Product
{
    internal class ProductVersionDo : IEntityVersionDo<ProductDo>
    {
        public ProductDo Entity { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}