using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.SupplierProduct
{
    public class SupplierProductLookup : SupplierProductBase
    {
        [Required]
        public string Id { get; set; }
    }
}