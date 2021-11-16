using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions
{
    internal interface IEntityWithIdDo
    {
        ObjectId Id { get; set; }
        long SeqId { get; set; }
    }
}