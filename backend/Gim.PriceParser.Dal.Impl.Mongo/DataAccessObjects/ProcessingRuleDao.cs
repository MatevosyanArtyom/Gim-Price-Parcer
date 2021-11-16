using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.ProcessingRules;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.Abstractions;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.ProcessingRule;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class ProcessingRuleDao : DaoBase<ProcessingRule, ProcessingRuleDo>, IProcessingRuleDao
    {
        public const string CollectionName = "ProcessingRules";

        private readonly IArchivableDao<ProcessingRuleDo> _archivableDao;

        public ProcessingRuleDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao,
            IArchivableDao<ProcessingRuleDo> archivableDao) : base(mapper, db, sequenceCounterDao, CollectionName)
        {
            _archivableDao = archivableDao;
            _archivableDao.Col = Col;
        }

        public async Task<GetAllResult<ProcessingRule>> GetManyAsync(ProcessingRuleFilter filter, SortParams sort,
            int page, int pageSize)
        {
            var filterDo = GimMapper.Map<FilterDefinition<ProcessingRuleDo>>(filter);
            var sortDo = GimMapper.Map<SortDefinition<ProcessingRuleDo>>(sort);

            var query = Col
                .Aggregate()
                .Match(filterDo)
                .Sort(sortDo)
                .Lookup<SupplierDo, ProcessingRuleFullDo>(
                    SupplierDao.CollectionName,
                    new ExpressionFieldDefinition<ProcessingRuleDo, ObjectId>(x => x.SupplierId),
                    new ExpressionFieldDefinition<SupplierDo, ObjectId>(x => x.Id),
                    new ExpressionFieldDefinition<ProcessingRuleFullDo, List<SupplierDo>>(x => x.Suppliers));

            query = query.Project<ProcessingRuleFullDo>(Builders<ProcessingRuleFullDo>.Projection.Exclude(x => x.Code));


            if (pageSize > 0)
            {
                query = query
                    .Skip(page * pageSize)
                    .Limit(pageSize);
            }

            var entities = await query.ToListAsync();

            var result = new GetAllResult<ProcessingRule>
            {
                Count = Col.CountDocuments(FilterDefinition<ProcessingRuleDo>.Empty),
                Entities = GimMapper.Map<List<ProcessingRule>>(entities)
            };

            return result;
        }

        public async Task ToArchiveOneAsync(string id)
        {
            await _archivableDao.ToArchiveOneAsync(id);
        }

        public async Task FromArchiveOneAsync(string id)
        {
            await _archivableDao.FromArchiveOneAsync(id);
        }
    }
}