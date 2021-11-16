using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Supplier
{
    public class SupplierLookup : SupplierBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }

        public string Region { get; set; }
        public string City { get; set; }
        public long ProductsCount { get; set; }

        [Required]
        public string User { get; set; }
    }
}