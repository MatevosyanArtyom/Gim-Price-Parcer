using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Product
{
    public class ProductLookup : ProductBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }

        public long SupplierCount { get; set; }
        public long ImageTotalCount { get; set; }
        public long ImagePublishedCount { get; set; }
    }
}