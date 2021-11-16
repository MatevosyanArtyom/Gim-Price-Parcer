using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier
{
    internal class SupplierVersionFullDo : IEntityWithIdDo
    {
        public SupplierFullDo Entity { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}