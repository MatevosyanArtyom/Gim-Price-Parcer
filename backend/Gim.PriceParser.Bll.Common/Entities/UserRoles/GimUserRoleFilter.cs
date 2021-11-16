using System;

namespace Gim.PriceParser.Bll.Common.Entities.UserRoles
{
    public class GimUserRoleFilter
    {
        public long? SeqId { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public string Name { get; set; }
        public int? UsersFrom { get; set; }
        public ArchivedFilter? ArchivedFilter { get; set; }
    }
}