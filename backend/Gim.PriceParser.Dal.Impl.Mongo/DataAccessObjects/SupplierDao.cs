using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Suppliers;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.Abstractions;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.User;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class SupplierDao : VersionDaoBase<Supplier, SupplierDo, SupplierVersionDo>, ISupplierDao
    {
        public const string CollectionName = "Suppliers";

        private readonly IArchivableDao<SupplierDo> _archivableDao;

        public SupplierDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao,
            IArchivableDao<SupplierDo> archivableDao, IHttpContextAccessor httpContextAccessor) : base(mapper, db,
            sequenceCounterDao, httpContextAccessor, CollectionName)
        {
            _archivableDao = archivableDao;
            _archivableDao.Col = Col;
        }

        public async Task<GetAllResult<Supplier>> GetManyAsync(SupplierFilter filter, SortParams sort, int page,
            int pageSize)
        {
            var filterDo = GimMapper.Map<FilterDefinition<SupplierDo>>(filter);
            var sortDo = GimMapper.Map<SortDefinition<SupplierDo>>(sort);
            var matched = Col
                .Aggregate()
                .Match(filterDo)
                .Sort(sortDo)
                .Lookup<UserDo, SupplierFullDo>(
                    UserDao.CollectionName,
                    nameof(SupplierDo.UserId),
                    nameof(UserDo.Id),
                    nameof(SupplierFullDo.Users));

            var countResult = await matched
                .Count()
                .FirstOrDefaultAsync();

            if (pageSize > 0)
            {
                matched = matched
                    .Skip(page * pageSize)
                    .Limit(pageSize);
            }

            var docsDo = await matched
                .As<SupplierFullDo>()
                .ToListAsync();

            var result = new GetAllResult<Supplier>
            {
                Count = countResult?.Count ?? 0,
                Entities = GimMapper.Map<List<Supplier>>(docsDo)
            };

            return result;
        }

        public async Task<long> CountAsync(SupplierFilter filter)
        {
            var filterDo = GimMapper.Map<FilterDefinition<SupplierDo>>(filter);
            var count = await Col.Find(filterDo).CountDocumentsAsync();
            return count;
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