using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.SupplierProduct
{
    public class SupplierProductBase
    {
        [Required]
        public string Supplier { get; set; }

        [Required]
        public long SupplierSeqId { get; set; }

        [Required]
        public string Product { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? Price1 { get; set; }
        public decimal? Price2 { get; set; }
        public decimal? Price3 { get; set; }
        public decimal? Quantity { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
    }
}