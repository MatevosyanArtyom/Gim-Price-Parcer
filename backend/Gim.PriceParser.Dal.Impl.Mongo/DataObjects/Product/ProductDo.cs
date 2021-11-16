using System.Collections.Generic;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Product
{
    internal class ProductDo : IEntityWithIdDo, IEntityWithVersionDo
    {
        public string Name { get; set; }
        public ObjectId CategoryId { get; set; }
        public ObjectId SupplierId { get; set; }
        public ObjectId ManufacturerId { get; set; }
        public string Description { get; set; }
        public List<ObjectId> Properties { get; set; }
        public EntityStatus Status { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
        public ObjectId Version { get; set; }
    }
}