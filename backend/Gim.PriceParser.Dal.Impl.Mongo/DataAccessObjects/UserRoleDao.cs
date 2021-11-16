using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.UserRoles;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.Abstractions;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.User;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.UserRole;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class UserRoleDao : DaoBase<GimUserRole, UserRoleDo>, IUserRoleDao
    {
        public const string CollectionName = "UserRoles";

        private readonly IArchivableDao<UserRoleDo> _archivableDao;


        public UserRoleDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao,
            IArchivableDao<UserRoleDo> archivableDao) : base(mapper, db,
            sequenceCounterDao, CollectionName)
        {
            _archivableDao = archivableDao;
            _archivableDao.Col = Col;
        }

        public async Task<GetAllResult<GimUserRole>> GetManyAsync(GimUserRoleFilter filter, SortParams sort, int page,
            int pageSize)
        {
            var filterDo = GimMapper.Map<FilterDefinition<UserRoleFullDo>>(filter);
            var sortDo = GimMapper.Map<SortDefinition<UserRoleFullDo>>(sort);

            var matched = Col
                .Aggregate()
                .Lookup<UserRoleDo, UserRoleFullDo>(
                    UserDao.CollectionName,
                    nameof(UserRoleDo.Id),
                    nameof(UserDo.RoleId),
                    nameof(UserRoleFullDo.Users))
                .Match(filterDo)
                .Sort(sortDo);

            var countResult = await matched
                .Count()
                .FirstOrDefaultAsync();

            if (pageSize > 0)
            {
                matched = matched
                    .Skip(page * pageSize)
                    .Limit(pageSize);
            }

            var docsDo = await matched.ToListAsync();

            var result = new GetAllResult<GimUserRole>
            {
                Count = countResult?.Count ?? 0,
                Entities = GimMapper.Map<List<GimUserRole>>(docsDo)
            };

            return result;
        }

        public async Task<GimUserRole> GetMainAdminAsync()
        {
            var filter = Builders<UserRoleDo>.Filter.Eq(x => x.IsMainAdmin, true);
            var docDo = await Col.Find(filter).FirstOrDefaultAsync();

            if (docDo == null)
            {
                return null;
            }

            var doc = GimMapper.Map<GimUserRole>(docDo);
            return doc;
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