using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Category
{
    public class CategoryEdit : CategoryFull
    {
        [Required]
        public string Id { get; set; }
    }
}