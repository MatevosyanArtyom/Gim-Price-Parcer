using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.WebApi.Models.Product;

namespace Gim.PriceParser.WebApi.Models.PriceListItem
{
    public class ProductSynonymDto
    {
        [Required]
        public ProductLookup Product { get; set; }

        [Required]
        public string ProductId { get; set; }

        public double Score { get; set; }
    }
}