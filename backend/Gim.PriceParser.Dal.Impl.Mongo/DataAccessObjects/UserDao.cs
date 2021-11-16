using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Users;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.Abstractions;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.User;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.UserRole;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class UserDao : DaoBase<GimUser, UserDo>, IUserDao
    {
        public const string CollectionName = "Users";

        private readonly IArchivableDao<UserDo> _archivableDao;

        public UserDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao,
            IArchivableDao<UserDo> archivableDao) : base(mapper, db,
            sequenceCounterDao, CollectionName)
        {
            _archivableDao = archivableDao;
            _archivableDao.Col = Col;
        }

        public async Task<GetAllResult<GimUser>> GetManyAsync(GimUserFilter filter, SortParams sort, int page,
            int pageSize)
        {
            var filterDo = GimMapper.Map<FilterDefinition<UserDo>>(filter);
            var sortDo = GimMapper.Map<SortDefinition<UserDo>>(sort);

            var matched = Col
                .Aggregate()
                .Match(filterDo)
                .Sort(sortDo)
                .Lookup<UserRoleDo, UserFullDo>(
                    UserRoleDao.CollectionName, 
                    nameof(UserDo.RoleId),
                    nameof(UserRoleDo.Id),
                    nameof(UserFullDo.Roles));

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
                .As<UserFullDo>()
                .ToListAsync();

            var result = new GetAllResult<GimUser>
            {
                Count = countResult?.Count ?? 0,
                Entities = GimMapper.Map<List<GimUser>>(docsDo)
            };

            return result;
        }

        public override async Task<GimUser> GetOneAsync(string id)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filterDo = Builders<UserDo>.Filter.Eq(x => x.Id, objId);
            return await GetOneWithFilterAsync(filterDo);
        }

        public async Task<GimUser> GetOneAsync(GimUserFilter filter)
        {
            var filterDo = GimMapper.Map<FilterDefinition<UserDo>>(filter);
            var doc = await GetOneWithFilterAsync(filterDo);
            return doc;
        }

        public async Task<GimUser> GetOneByEmailAsync(string email)
        {
            var filterDo = Builders<UserDo>.Filter.Eq(x => x.Email, email);
            var doc = await GetOneWithFilterAsync(filterDo);
            return doc;
        }

        public async Task<GimUser> SetPasswordTokenAsync(string id)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var token = ObjectId.GenerateNewId();

            var filter = Builders<UserDo>.Filter.Eq(x => x.Id, objId);
            var update = Builders<UserDo>.Update.Set(x => x.ChangePasswordToken, token);
            await Col.UpdateOneAsync(filter, update);

            return await GetOneAsync(id);
        }

        public override async Task<GimUser> UpdateOneAsync(GimUser entity)
        {
            await base.UpdateOneAsync(entity);
            return await GetOneByEmailAsync(entity.Email);
        }

        public async Task ToArchiveOneAsync(string id)
        {
            await _archivableDao.ToArchiveOneAsync(id);
        }

        public async Task FromArchiveOneAsync(string id)
        {
            await _archivableDao.FromArchiveOneAsync(id);
        }

        private async Task<GimUser> GetOneWithFilterAsync(FilterDefinition<UserDo> filterDo)
        {
            var docDo = await Col
                .Aggregate()
                .Match(filterDo)
                .Lookup<UserRoleDo, UserFullDo>(
                    UserRoleDao.CollectionName,
                    nameof(UserDo.RoleId),
                    nameof(UserRoleDo.Id),
                    nameof(UserFullDo.Roles))
                .As<UserFullDo>()
                .FirstOrDefaultAsync();

            if (docDo == null)
            {
                return null;
            }

            var doc = GimMapper.Map<GimUser>(docDo);
            return doc;
        }
    }
}