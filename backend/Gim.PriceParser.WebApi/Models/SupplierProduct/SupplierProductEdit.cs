using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.SupplierProduct
{
    public class SupplierProductEdit : SupplierProductBase
    {
        [Required]
        public string Id { get; set; }
    }
}