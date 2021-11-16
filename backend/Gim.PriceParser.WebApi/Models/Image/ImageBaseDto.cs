using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities.Images;

namespace Gim.PriceParser.WebApi.Models.Image
{
    public class ImageBaseDto
    {
        public bool IsMain { get; set; }
        public bool IsPublished { get; set; }

        [Required]
        public string ProductId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        [Required]
        public int Size { get; set; }

        public GimImageDownloadStatus Status { get; set; }
    }
}