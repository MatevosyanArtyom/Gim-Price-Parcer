using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.UserRole;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.User
{
    internal class UserFullDo : UserDo
    {
        public List<UserRoleDo> Roles { get; set; }
    }
}