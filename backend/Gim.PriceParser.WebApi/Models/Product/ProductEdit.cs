using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Product
{
    public class ProductEdit : ProductBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }
    }
}