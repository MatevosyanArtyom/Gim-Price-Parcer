using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.EntityVersion;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.User;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class VersionDaoBase<T, TDo, TVersionDo> : DaoBase<T, TDo>, IVersionDaoBase<T>
        where TDo : IEntityWithIdDo, IEntityWithVersionDo where TVersionDo : IEntityWithIdDo, IEntityWithVersionDo
    {
        private readonly string _collectionName;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISequenceCounterDao _sequenceCounterDao;
        protected readonly IMongoCollection<EntityVersionDo<TVersionDo>> VersionsCol;

        public VersionDaoBase(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao,
            IHttpContextAccessor httpContextAccessor, string collectionName) : base(mapper, db, sequenceCounterDao,
            collectionName)
        {
            _httpContextAccessor = httpContextAccessor;
            _sequenceCounterDao = sequenceCounterDao;
            _collectionName = $"{collectionName}Versions";
            VersionsCol = Db.GetCollection<EntityVersionDo<TVersionDo>>(_collectionName);
        }

        public override async Task<T> AddOneAsync(T entity)
        {
            var doc = await base.AddOneAsync(entity);
            var docDo = GimMapper.Map<TVersionDo>(doc);

            await AddVersion(docDo);

            return doc;
        }

        public override async Task<List<T>> AddManyAsync(List<T> entities)
        {
            var docsDo = GimMapper.Map<List<TDo>>(entities);
            await Col.InsertManyAsync(docsDo);

            var versionsDo = GimMapper.Map<List<EntityVersionDo<TVersionDo>>>(docsDo);
            await VersionsCol.InsertManyAsync(versionsDo);

            var result = GimMapper.Map<List<T>>(docsDo);
            return result;
        }

        public override async Task<T> UpdateOneAsync(T entity)
        {
            var doc = await base.UpdateOneAsync(entity);

            var versionDo = GimMapper.Map<TVersionDo>(doc);
            await AddVersion(versionDo);

            return doc;
        }

        public virtual async Task<GetAllResult<EntityVersion<T>>> GetVersions(string id, int page, int pageSize)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filter = Builders<EntityVersionDo<TVersionDo>>.Filter.Eq(x => x.Entity.Id, objId);
            var sort = Builders<EntityVersionDo<TVersionDo>>.Sort.Descending(x => x.Id);

            var filtered = VersionsCol.Aggregate()
                .Match(filter)
                .Sort(sort)
                .Lookup<EntityVersionDo<TVersionDo>, EntityVersionFullDo<TVersionDo>>(
                    UserDao.CollectionName,
                    nameof(EntityVersionDo<TVersionDo>.UserId),
                    nameof(UserDo.Id),
                    nameof(EntityVersionFullDo<TVersionDo>.Users));

            var countResult = await filtered.Count().FirstOrDefaultAsync();

            var docsDo = await filtered
                .Skip(page * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var result = new GetAllResult<EntityVersion<T>>
            {
                Count = countResult?.Count ?? 0,
                Entities = GimMapper.Map<List<EntityVersion<T>>>(docsDo)
            };

            return result;
        }

        public virtual async Task<T> RestoreVersion(string versionId)
        {
            var versionObjId = GimMapper.Map<ObjectId>(versionId);
            var versionDo = await VersionsCol
                .Find(e => e.Id == versionObjId)
                .FirstOrDefaultAsync();
            var documentDo = GimMapper.Map<TDo>(versionDo);
            await Col.FindOneAndReplaceAsync(docDo => docDo.Id == documentDo.Id, documentDo);

            return GimMapper.Map<T>(documentDo);
        }

        private async Task AddVersion(TVersionDo entity)
        {
            var userId = GimMapper.Map<ObjectId>(_httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value);

            var counter = await _sequenceCounterDao.GetCounterAsync(_collectionName);

            var docDo = GimMapper.Map<EntityVersionDo<TVersionDo>>(entity);
            docDo.SeqId = ++counter;
            docDo.UserId = userId;
            await VersionsCol.InsertOneAsync(docDo);

            await _sequenceCounterDao.SetCounterAsync(_collectionName, counter);
        }
    }
}