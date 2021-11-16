using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Image
{
    public class ImageLookupDto : ImageBaseDto
    {
        [Required]
        public string Id { get; set; }
    }
}