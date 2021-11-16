using System.Collections.Generic;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceListItem
{
    internal class PriceListItemDo : IEntityWithIdDo
    {
        public ObjectId PriceListId { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public ObjectId ProductId { get; set; }
        public List<ProductSynonymDo> ProductSynonyms { get; set; }
        public string Category1Name { get; set; }
        public ObjectId Category1Id { get; set; }
        public PriceListItemStatus Category1Status { get; set; }
        public PriceListItemCategoryAction Category1Action { get; set; }
        public ObjectId MapTo1Id { get; set; }
        public string Category2Name { get; set; }
        public ObjectId Category2Id { get; set; }
        public PriceListItemStatus Category2Status { get; set; }
        public PriceListItemCategoryAction Category2Action { get; set; }
        public ObjectId MapTo2Id { get; set; }
        public string Category3Name { get; set; }
        public ObjectId Category3Id { get; set; }
        public PriceListItemStatus Category3Status { get; set; }
        public PriceListItemCategoryAction Category3Action { get; set; }
        public ObjectId MapTo3Id { get; set; }
        public string Category4Name { get; set; }
        public ObjectId Category4Id { get; set; }
        public PriceListItemStatus Category4Status { get; set; }
        public PriceListItemCategoryAction Category4Action { get; set; }
        public ObjectId MapTo4Id { get; set; }
        public string Category5Name { get; set; }
        public ObjectId Category5Id { get; set; }
        public PriceListItemStatus Category5Status { get; set; }
        public PriceListItemCategoryAction Category5Action { get; set; }
        public ObjectId MapTo5Id { get; set; }
        public ObjectId SupplierProductId { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price1 { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? Price2 { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? Price3 { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? Quantity { get; set; }

        public string Description { get; set; }

        public PriceListItemAction NameAction { get; set; }
        public PriceListItemStatus NameStatus { get; set; }

        public PriceListItemStatus Status { get; set; }
        public bool Skip { get; set; }

        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}