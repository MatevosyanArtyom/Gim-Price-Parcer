using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Supplier
{
    public class SupplierShort
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}