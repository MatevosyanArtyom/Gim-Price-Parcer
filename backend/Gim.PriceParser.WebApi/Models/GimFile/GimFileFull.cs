using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.GimFile
{
    public class GimFileFull : GimFileBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Data { get; set; }
    }
}