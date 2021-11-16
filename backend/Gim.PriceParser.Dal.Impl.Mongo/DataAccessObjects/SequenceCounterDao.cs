using System.Threading.Tasks;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class SequenceCounterDao : ISequenceCounterDao
    {
        public const string CollectionName = "SequenceCounters";

        private readonly IMongoCollection<SequenceCounterDo> _col;

        public SequenceCounterDao(IGimDbContext db)
        {
            _col = db.GetCollection<SequenceCounterDo>(CollectionName);
        }

        public async Task<long> GetCounterAsync(string name)
        {
            var filter = Builders<SequenceCounterDo>.Filter.Eq(x => x.Name, name);
            var counterDo = await _col.Find(filter).FirstOrDefaultAsync();
            return counterDo?.Counter ?? 0;
        }

        public async Task SetCounterAsync(string name, long counter)
        {
            var filter = Builders<SequenceCounterDo>.Filter.Eq(x => x.Name, name);
            var update = Builders<SequenceCounterDo>.Update.Set(x => x.Counter, counter);
            var options = new FindOneAndUpdateOptions<SequenceCounterDo> {IsUpsert = true};
            await _col.FindOneAndUpdateAsync(filter, update, options);
        }
    }
}