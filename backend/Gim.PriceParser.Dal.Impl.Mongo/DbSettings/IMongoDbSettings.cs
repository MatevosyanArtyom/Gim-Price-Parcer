using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DbSettings
{
    public interface IMongoDbSettings
    {
        string ConnectionString { get; set; }
        string Host { get; set; }
        int Port { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string DatabaseName { get; set; }

        MongoClientSettings GetMongoClientSettings();
    }
}