using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceList;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.ProcessingRule;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SchedulerTask;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.User;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class PriceListDao : DaoBase<PriceList, PriceListDo>, IPriceListDao
    {
        public const string CollectionName = "PriceLists";

        public PriceListDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao) : base(mapper, db,
            sequenceCounterDao, CollectionName)
        {
        }

        public async Task<GetAllResult<PriceList>> GetManyAsync(PriceListFilter filter, SortParams sort,
            bool withData = false, int page = 0, int pageSize = 0)
        {
            var filterDo = GimMapper.Map<FilterDefinition<PriceListDo>>(filter);
            var sortDo = GimMapper.Map<SortDefinition<PriceListDo>>(sort);

            var query = Col
                .Aggregate()
                .Match(filterDo)
                .Sort(sortDo)
                .Lookup<SupplierDo, PriceListFullDo>(
                    SupplierDao.CollectionName,
                    new ExpressionFieldDefinition<PriceListDo, ObjectId>(x => x.SupplierId),
                    new ExpressionFieldDefinition<SupplierDo, ObjectId>(x => x.Id),
                    new ExpressionFieldDefinition<PriceListFullDo, List<SupplierDo>>(x => x.Suppliers))
                .Lookup<SchedulerTaskDo, PriceListFullDo>(
                    SchedulerTaskDao.CollectionName,
                    new ExpressionFieldDefinition<PriceListFullDo, ObjectId>(x => x.SchedulerTaskId),
                    new ExpressionFieldDefinition<SchedulerTaskDo, ObjectId>(x => x.Id),
                    new ExpressionFieldDefinition<PriceListFullDo, List<SchedulerTaskDo>>(x => x.SchedulerTasks))
                .Lookup<ProcessingRuleDo, PriceListFullDo>(
                    ProcessingRuleDao.CollectionName,
                    new ExpressionFieldDefinition<PriceListFullDo, ObjectId>(x => x.ProcessingRuleId),
                    new ExpressionFieldDefinition<ProcessingRuleDo, ObjectId>(x => x.Id),
                    new ExpressionFieldDefinition<PriceListFullDo, List<ProcessingRuleDo>>(x => x.ProcessingRules));

            if (!withData)
            {
                query = query.Project<PriceListFullDo>(Builders<PriceListFullDo>.Projection
                    .Exclude(x => x.PriceListFile.Data)
                );
            }

            if (pageSize > 0)
            {
                query = query
                    .Skip(page * pageSize)
                    .Limit(pageSize);
            }

            var entities = await query
                .As<PriceListFullDo>()
                .ToListAsync();

            var result = new GetAllResult<PriceList>
            {
                Count = Col.CountDocuments(FilterDefinition<PriceListDo>.Empty),
                Entities = GimMapper.Map<List<PriceList>>(entities)
            };

            return result;
        }

        public async Task SetStatusAsync(string id, PriceListStatus status)
        {
            var objId = GimMapper.Map<ObjectId>(id);

            var filter = Builders<PriceListDo>.Filter.Eq(x => x.Id, objId);
            var update = Builders<PriceListDo>.Update
                .Set(x => x.Status, status)
                .Set(x => x.StatusDate, DateTime.Now);

            await Col.UpdateOneAsync(filter, update);
        }

        public async Task<PriceList> GetOneAsync(string id, bool excludeData)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filterDo = Builders<PriceListDo>.Filter.Eq(x => x.Id, objId);

            var query = Col
                .Aggregate()
                .Match(filterDo)
                .Lookup<SupplierDo, PriceListFullDo>(
                    SupplierDao.CollectionName,
                    new ExpressionFieldDefinition<PriceListDo, ObjectId>(x => x.SupplierId),
                    new ExpressionFieldDefinition<SupplierDo, ObjectId>(x => x.Id),
                    nameof(PriceListFullDo.Suppliers))
                .Lookup<ProcessingRuleDo, PriceListFullDo>(
                    ProcessingRuleDao.CollectionName,
                    nameof(PriceListDo.ProcessingRuleId),
                   nameof(ProcessingRuleDo.Id),
                    nameof(PriceListFullDo.ProcessingRules))
                .Lookup<UserDo, PriceListFullDo>(
                    UserDao.CollectionName,
                    nameof(PriceListDo.AuthorId),
                    nameof(UserDo.Id),
                    nameof(PriceListFullDo.Authors));

            if (excludeData)
            {
                var projections = new List<ProjectionDefinition<PriceListFullDo>>();

                if (excludeData)
                {
                    projections.Add(Builders<PriceListFullDo>.Projection.Exclude(x => x.PriceListFile.Data));
                }

                query = query.Project<PriceListFullDo>(Builders<PriceListFullDo>.Projection.Combine(projections));
            }

            var docDo = await query
                .As<PriceListFullDo>()
                .FirstAsync();

            var doc = GimMapper.Map<PriceList>(docDo);
            return doc;
        }
    }
}