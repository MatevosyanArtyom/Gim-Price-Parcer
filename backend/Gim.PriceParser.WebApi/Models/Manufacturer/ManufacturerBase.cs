using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Manufacturer
{
    public abstract class ManufacturerBase
    {
        [Required]
        public string Name { get; set; }
    }
}