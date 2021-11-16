using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryProperty;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryPropertyValue;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceListItem;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class PriceListItemPropertyDao :
        DaoBase<PriceListItemPropertyMatched, PriceListItemPropertyDo>, IPriceListItemPropertyDao
    {
        public const string CollectionName = "PriceListItemProperties";

        public PriceListItemPropertyDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao)
            : base(mapper, db, sequenceCounterDao, CollectionName)
        {
        }

        public async Task<GetAllResult<PriceListItemPropertyMatched>> GetManyIndexedAsync(
            PriceListItemPropertyFilter filter,
            int startIndex = 0, int stopIndex = 0)
        {
            var filterDo = GimMapper.Map<FilterDefinition<PriceListItemPropertyDo>>(filter);
            var query = Col
                .Aggregate()
                .Match(filterDo);

            if (stopIndex > 0)
            {
                query = query
                    .Skip(startIndex)
                    .Limit(stopIndex - startIndex + 1);
            }

            query = query
                .Lookup<CategoryPropertyDo, PriceListItemPropertyDo>(
                    CategoryPropertyDao.CollectionName,
                    nameof(PriceListItemPropertyDo.PropertyId),
                    nameof(CategoryPropertyDo.Id),
                    nameof(PriceListItemPropertyFullDo.Properties))
                .Lookup<CategoryPropertyValueDo, PriceListItemPropertyDo>(
                    CategoryPropertyValueDao.CollectionName,
                    nameof(PriceListItemPropertyDo.ValueId),
                    nameof(CategoryPropertyValueDo.Id),
                    nameof(PriceListItemPropertyFullDo.Values));

            var entitiesDo = await query
                .As<PriceListItemPropertyFullDo>()
                .ToListAsync();

            var entities = GimMapper.Map<List<PriceListItemPropertyMatched>>(entitiesDo);

            var result = new GetAllResult<PriceListItemPropertyMatched>
            {
                Count = Col.CountDocuments(filterDo),
                Entities = entities
            };

            return result;
        }

        public async Task SetActionOneAsync(string id, PriceListItemAction action)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filter = Builders<PriceListItemPropertyDo>.Filter.Eq(x => x.Id, objId);
            var update = Builders<PriceListItemPropertyDo>.Update.Set(x => x.Action, action);

            await Col.UpdateOneAsync(filter, update);
        }

        public async Task SetActionManyAsync(PriceListItemPropertyFilter filter, PriceListItemAction action)
        {
            var filterDo = GimMapper.Map<FilterDefinition<PriceListItemPropertyDo>>(filter);
            var updateDo = Builders<PriceListItemPropertyDo>.Update.Set(x => x.Action, action);

            await Col.UpdateManyAsync(filterDo, updateDo);
        }

        public async Task DeleteManyAsync(PriceListItemPropertyFilter filter)
        {
            var filterDo = GimMapper.Map<FilterDefinition<PriceListItemPropertyDo>>(filter);

            await Col.DeleteManyAsync(filterDo);
        }
    }
}