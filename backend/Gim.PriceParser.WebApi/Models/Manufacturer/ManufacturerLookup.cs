using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Manufacturer
{
    public class ManufacturerLookup : ManufacturerBase
    {
        [Required]
        public string Id { get; set; }
    }
}