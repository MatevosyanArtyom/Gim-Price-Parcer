using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Manufacturer
{
    public class ManufacturerEdit : ManufacturerFull
    {
        [Required]
        public string Id { get; set; }
    }
}