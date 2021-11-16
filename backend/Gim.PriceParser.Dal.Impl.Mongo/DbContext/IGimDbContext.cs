using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DbContext
{
    public interface IGimDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}