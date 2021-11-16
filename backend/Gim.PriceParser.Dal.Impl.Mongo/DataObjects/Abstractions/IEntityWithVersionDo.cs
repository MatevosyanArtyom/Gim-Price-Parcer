using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions
{
    internal interface IEntityWithVersionDo
    {
        ObjectId Version { get; set; }
    }
}