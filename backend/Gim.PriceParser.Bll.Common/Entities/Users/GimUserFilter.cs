using System;

namespace Gim.PriceParser.Bll.Common.Entities.Users
{
    public class GimUserFilter
    {
        public long? SeqId { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public GimUserStatus? Status { get; set; }
        public string Token { get; set; }
        public ArchivedFilter ArchivedFilter { get; set; }
    }
}