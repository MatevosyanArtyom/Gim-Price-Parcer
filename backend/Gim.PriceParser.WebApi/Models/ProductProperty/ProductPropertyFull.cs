using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.ProductProperty
{
    public class ProductPropertyFull : ProductPropertyBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }
    }
}