using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.GimFile
{
    public class GimFileBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Size { get; set; }
    }
}