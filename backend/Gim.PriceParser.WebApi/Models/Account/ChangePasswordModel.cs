using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Account
{
    public class ChangePasswordModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [StringLength(24, MinimumLength = 8)]
        [RegularExpression(@"^.*(?=.*\d)(?=.*[a-zA-Z]).*$", ErrorMessage =
            "Password must contain at least one number and alphabetical character")]
        public string Password { get; set; }
    }
}