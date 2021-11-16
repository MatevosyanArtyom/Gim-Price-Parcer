using Gim.PriceParser.Dal.Impl.Mongo.DbSettings;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DbContext
{
    public class GimDbContext : IGimDbContext
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoDbSettings _dbSettings;

        public GimDbContext(IMongoDbSettings mongoDbSettings)
        {
            _dbSettings = mongoDbSettings;

            var mongo = GetMongoClient();

            _db = mongo.GetDatabase(mongoDbSettings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }

        public void Init()
        {
            //var mongo = getMongoClient();
            //mongo.
        }

        private MongoClient GetMongoClient()
        {
            var mongo = new MongoClient(_dbSettings.GetMongoClientSettings());
            return mongo;
        }
    }
}