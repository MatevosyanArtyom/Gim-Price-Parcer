using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DbSettings
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }

        public MongoClientSettings GetMongoClientSettings()
        {
            MongoClientSettings settings;

            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                settings = MongoClientSettings.FromConnectionString(ConnectionString);
            }
            else
            {
                settings = new MongoClientSettings
                {
                    Server = new MongoServerAddress(Host, Port),
                    Credential = MongoCredential.CreateCredential(DatabaseName, Username, Password)
                };
            }

            return settings;
        }
    }
}