using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Dal.Impl.Mongo.Abstractions;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class ArchivableDao<TDo> : IArchivableDao<TDo> where TDo : IEntityWithIdDo, IEntityArchivableDo
    {
        private readonly IMapper _mapper;

        public ArchivableDao(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IMongoCollection<TDo> Col { get; set; }

        public async Task ToArchiveOneAsync(string id)
        {
            await SetIsArchived(id, true);
        }

        public async Task FromArchiveOneAsync(string id)
        {
            await SetIsArchived(id, false);
        }

        private async Task SetIsArchived(string id, bool value)
        {
            var objId = _mapper.Map<ObjectId>(id);
            var filter = Builders<TDo>.Filter.Eq(x => x.Id, objId);
            var update = Builders<TDo>.Update.Set(x => x.IsArchived, value);
            await Col.UpdateOneAsync(filter, update);
        }
    }
}