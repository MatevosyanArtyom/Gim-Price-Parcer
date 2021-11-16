using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.ProductPropertyValue
{
    public class ProductPropertyValueLookup : ProductPropertyValueFull
    {
        [Required]
        public string PropertyId { get; set; }
    }
}