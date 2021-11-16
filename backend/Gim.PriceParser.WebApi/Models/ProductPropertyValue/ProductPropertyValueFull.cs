using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.ProductPropertyValue
{
    public class ProductPropertyValueFull : ProductPropertyValueBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }
    }
}