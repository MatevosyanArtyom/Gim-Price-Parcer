using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.SchedulerTasks;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SchedulerTask;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class SchedulerTaskDao : DaoBase<SchedulerTask, SchedulerTaskDo>, ISchedulerTaskDao
    {
        public const string CollectionName = "SchedulerTasks";

        public SchedulerTaskDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao) : base(mapper,
            db, sequenceCounterDao, CollectionName)
        {
        }

        public override async Task<GetAllResult<SchedulerTask>> GetManyAsync(int page, int pageSize)
        {
            return await GetManyAsync(new SchedulerTaskFilter(), page, pageSize);
        }

        public async Task<GetAllResult<SchedulerTask>> GetManyAsync(SchedulerTaskFilter filter, int page = 0,
            int pageSize = 0)
        {
            var filterDo = GimMapper.Map<FilterDefinition<SchedulerTaskDo>>(filter);
            var query = Col
                .Aggregate()
                .Match(filterDo)
                .Lookup<SupplierDo, SchedulerTaskFullDo>(
                    SupplierDao.CollectionName,
                    new ExpressionFieldDefinition<SchedulerTaskDo, ObjectId>(x => x.SupplierId),
                    new ExpressionFieldDefinition<SupplierDo, ObjectId>(x => x.Id),
                    new ExpressionFieldDefinition<SchedulerTaskFullDo, List<SupplierDo>>(x => x.Suppliers));

            if (pageSize > 0)
            {
                query = query
                    .Skip(page * pageSize)
                    .Limit(pageSize);
            }

            var entities = await query
                .As<SchedulerTaskFullDo>()
                .ToListAsync();

            var result = new GetAllResult<SchedulerTask>
            {
                Count = Col.CountDocuments(FilterDefinition<SchedulerTaskDo>.Empty),
                Entities = GimMapper.Map<List<SchedulerTask>>(entities)
            };

            return result;
        }
    }
}