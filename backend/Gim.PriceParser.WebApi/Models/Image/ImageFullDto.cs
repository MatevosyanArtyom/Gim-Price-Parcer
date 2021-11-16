using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Image
{
    public class ImageFullDto : ImageLookupDto
    {
        [Required]
        public byte[] Data { get; set; }
    }
}