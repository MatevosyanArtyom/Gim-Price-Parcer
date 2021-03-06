using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.UserRole
{
    public class UserRoleEdit : UserRoleFull
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }
    }
}