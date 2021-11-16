using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceListItem
{
    internal class PriceListItemImageDo : IEntityWithIdDo
    {
        public ObjectId PriceListItemId { get; set; }
        public ObjectId ImageId { get; set; }
        public string Url { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}