using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.UserRole
{
    public class UserRoleLookup : UserRoleBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }

        public int UsersCount { get; set; }
    }
}