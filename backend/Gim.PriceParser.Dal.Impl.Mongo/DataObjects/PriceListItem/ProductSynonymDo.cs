using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceListItem
{
    public class ProductSynonymDo
    {
        public ObjectId ProductId { get; set; }
        public double Score { get; set; }
    }
}