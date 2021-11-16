using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.User;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.UserRole
{
    internal class UserRoleFullDo : UserRoleDo
    {
        public IEnumerable<UserDo> Users { get; set; }
        public int UsersCount { get; set; }
    }
}