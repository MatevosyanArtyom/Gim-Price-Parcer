using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceListItem
{
    internal class PriceListItemPropertyDo : IEntityWithIdDo
    {
        public ObjectId PriceListItemId { get; set; }
        public string PropertyKey { get; set; }
        public ObjectId PropertyId { get; set; }
        public string PropertyValue { get; set; }
        public ObjectId ValueId { get; set; }
        public PriceListItemAction Action { get; set; }
        public PriceListItemStatus Status { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}