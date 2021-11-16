using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Category
{
    public class UpdateParentModel
    {
        [Required]
        public string Id { get; set; }

        public string NewParentId { get; set; }
    }
}