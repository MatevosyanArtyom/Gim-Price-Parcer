using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects
{
    internal class ManufacturerDo : IEntityWithIdDo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}