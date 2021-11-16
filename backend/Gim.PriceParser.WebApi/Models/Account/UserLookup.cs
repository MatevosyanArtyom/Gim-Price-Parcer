using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.WebApi.Models.UserRole;

namespace Gim.PriceParser.WebApi.Models.Account
{
    public class UserLookup : UserBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }

        [Required]
        public AccessRightsDto AccessRights { get; set; }
    }
}