using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Account
{
    public abstract class UserFull : UserBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }
    }
}