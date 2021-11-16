using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SupplierProduct
{
    internal class SupplierProductDo : IEntityWithIdDo
    {
        public ObjectId SupplierId { get; set; }
        public ObjectId ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price1 { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? Price2 { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? Price3 { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? Quantity { get; set; }

        public string Description { get; set; }

        public List<ObjectId> Properties { get; set; }

        public ObjectId Version { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}