using System;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities.UserRoles;
using Gim.PriceParser.Bll.Common.Entities.Users;
using Gim.PriceParser.Dal.Common;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;

namespace Gim.PriceParser.Dal.Impl.Mongo
{
    internal class DbSeeder : IDbSeeder
    {
        private readonly IUserDao _userDao;
        private readonly IUserRoleDao _userRoleDao;

        public DbSeeder(IUserRoleDao userRoleDao, IUserDao userDao, IGimDbContext db)
        {
            _userRoleDao = userRoleDao;
            _userDao = userDao;
        }

        public async Task Seed()
        {
            var adminRole = await SeedAdminRoleAsync();

            await SeedAdminUserAsync(adminRole.Id);
        }

        private async Task<GimUserRole> SeedAdminRoleAsync()
        {
            var adminRole = await _userRoleDao.GetMainAdminAsync();

            if (adminRole != null)
            {
                return adminRole;
            }

            var newAdminRole = new GimUserRole
            {
                Name = "Главный администратор",
                IsMainAdmin = true,
                CreatedDate = DateTime.Today,
                IsArchived = false,
                AccessRights = new AccessRights
                {
                    Categories = new AccessRightMode {Edit = true, Full = true, Read = true},
                    CommitedPriceLists = new AccessRightMode {Read = true},
                    PriceListAdd = new AccessRightMode {Full = true},
                    PriceLists = new AccessRightMode
                        {Read = true, EditSelf = true, Full = true, CreateProperties = true},
                    ProcessingRules = new AccessRightMode {Read = true, Full = true},
                    Products = new AccessRightMode {Read = true, Full = true},
                    Properties = new AccessRightMode {Read = true, Full = true},
                    Suppliers = new AccessRightMode {ReadSelf = true, EditSelf = true, Read = true, Full = true},
                    UserRoles = new AccessRightMode {Read = true, Full = true},
                    Users = new AccessRightMode {Read = true, Full = true}
                }
            };
            newAdminRole = await _userRoleDao.AddOneAsync(newAdminRole);
            return newAdminRole;
        }

        private async Task SeedAdminUserAsync(string adminRoleId)
        {
            var filter = new GimUserFilter
            {
                RoleId = adminRoleId
            };

            var adminUser = await _userDao.GetOneAsync(filter);

            if (adminUser != null)
            {
                return;
            }

            var newAdminUser = new GimUser
            {
                Email = "admin@example.com",
                Password =
                    "AQAAAAEAACcQAAAAEK9h3XLKPNtYzRhLW5pzDAaaWU3d+UT/gbTYuEm7oc7ahyD1WGOgLGHfov/7Uaz8jg==", // admin1234
                Fullname = "",
                PhoneNumber = "",
                Position = "",
                RoleId = adminRoleId,
                HasSuppliersAccess = true,
                HasFullAccess = true,
                HasUsersAccess = true,
                CreatedDate = DateTime.Today,
                IsArchived = false,
                Status = GimUserStatus.Active
            };
            await _userDao.AddOneAsync(newAdminUser);
        }
    }
}