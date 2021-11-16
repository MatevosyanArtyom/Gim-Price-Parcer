using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class DaoBase<T, TDo> : IDaoBase<T> where TDo : IEntityWithIdDo
    {
        private readonly string _collectionName;
        private readonly ISequenceCounterDao _sequenceCounterDao;
        protected readonly IMongoCollection<TDo> Col;
        protected readonly IGimDbContext Db;
        protected readonly IMapper GimMapper;

        public DaoBase(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao, string collectionName)
        {
            GimMapper = mapper;
            Db = db;
            _sequenceCounterDao = sequenceCounterDao;
            _collectionName = collectionName;
            Col = Db.GetCollection<TDo>(collectionName);
        }

        public virtual async Task<GetAllResult<T>> GetManyAsync(int page, int pageSize)
        {
            var filtered = Col.Find(FilterDefinition<TDo>.Empty);

            var count = await filtered.CountDocumentsAsync();

            var docsDo = await filtered
                .Skip(page * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var result = new GetAllResult<T>
            {
                Count = count,
                Entities = GimMapper.Map<List<T>>(docsDo)
            };

            return result;
        }

        public async Task<List<string>> GetIds()
        {
            var ids = await Col
                .Find(FilterDefinition<TDo>.Empty)
                .Project(x => x.Id.ToString())
                .ToListAsync();

            return ids;
        }

        public string GenerateNewObjectId()
        {
            return GimMapper.Map<string>(ObjectId.GenerateNewId());
        }

        public virtual async Task<T> GetOneAsync(string id)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filter = Builders<TDo>.Filter.Eq(nameof(IEntityWithIdDo.Id), objId);
            var docDo = await Col.Find(filter).FirstAsync();
            var doc = GimMapper.Map<T>(docDo);
            return doc;
        }

        public virtual async Task<T> AddOneAsync(T entity)
        {
            var docDo = GimMapper.Map<TDo>(entity);

            var counter = await _sequenceCounterDao.GetCounterAsync(_collectionName);
            docDo.SeqId = ++counter;

            await Col.InsertOneAsync(docDo);
            await _sequenceCounterDao.SetCounterAsync(_collectionName, counter);

            var doc = GimMapper.Map<T>(docDo);
            return doc;
        }

        public virtual async Task<List<T>> AddManyAsync(List<T> entities)
        {
            if (!entities.Any())
            {
                return entities;
            }

            var docsDo = GimMapper.Map<List<TDo>>(entities);

            var counter = await _sequenceCounterDao.GetCounterAsync(_collectionName);
            docsDo.ForEach(docDo => docDo.SeqId = ++counter);

            await Col.InsertManyAsync(docsDo);
            await _sequenceCounterDao.SetCounterAsync(_collectionName, counter);

            var docs = GimMapper.Map<List<T>>(docsDo);
            return docs;
        }

        public virtual async Task<T> UpdateOneAsync(T entity)
        {
            var docDo = GimMapper.Map<TDo>(entity);
            await Col.ReplaceOneAsync(s => s.Id == docDo.Id, docDo);
            var doc = GimMapper.Map<T>(docDo);
            return doc;
        }

        public virtual async Task DeleteOneAsync(string id)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            await Col.DeleteOneAsync(entity => entity.Id == objId);
        }
    }
}