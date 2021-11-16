using System.Collections.Generic;
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
    internal class DaoWithVersionsBase<T, TDo, TVersionDo> : DaoBase<T, TDo>, IDaoWithVersionsBase<T>
        where TDo : IEntityWithIdDo, IEntityWithVersionDo where TVersionDo : IEntityVersionDo<TDo>
    {
        protected readonly IMongoCollection<TVersionDo> VersionsCol;

        public DaoWithVersionsBase(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao,
            string collectionName) : base(mapper, db, sequenceCounterDao, collectionName)
        {
            VersionsCol = Db.GetCollection<TVersionDo>($"{collectionName}Versions");
        }

        public override async Task<T> AddOneAsync(T entity)
        {
            var doc = await base.AddOneAsync(entity);
            var docDo = GimMapper.Map<TDo>(doc);

            await AddVersion(docDo);

            return doc;
        }

        public override async Task<List<T>> AddManyAsync(List<T> entities)
        {
            var docsDo = GimMapper.Map<List<TDo>>(entities);
            await base.AddManyAsync(entities);

            var versionsDo = GimMapper.Map<List<TVersionDo>>(docsDo);
            await VersionsCol.InsertManyAsync(versionsDo);

            var result = GimMapper.Map<List<T>>(docsDo);
            return result;
        }

        public override async Task<T> UpdateOneAsync(T entity)
        {
            var doc = await base.UpdateOneAsync(entity);
            var docDo = GimMapper.Map<TDo>(doc);

            await AddVersion(docDo);

            return doc;
        }

        public virtual async Task<GetAllResult<T>> GetVersions(string id, int page, int pageSize)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filter = Builders<TVersionDo>.Filter.Eq(x => x.Id, objId);

            var filtered = VersionsCol.Find(filter);

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

        public virtual async Task<T> RestoreVersion(string id, string version)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var versionObjId = GimMapper.Map<ObjectId>(version);
            var versionDo = await VersionsCol
                .Find(e => e.Entity.Id == objId && e.Entity.Version == versionObjId)
                .FirstOrDefaultAsync();
            var documentDo = GimMapper.Map<TDo>(versionDo);
            await Col.FindOneAndReplaceAsync(docDo => docDo.Id == documentDo.Id, documentDo);

            return GimMapper.Map<T>(documentDo);
        }

        private async Task AddVersion(TDo entity)
        {
            var docDo = GimMapper.Map<TVersionDo>(entity);
            docDo.Id = ObjectId.Empty;
            await VersionsCol.InsertOneAsync(docDo);
        }
    }
}