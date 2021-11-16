using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.GimFile
{
    public class GimFileAdd : GimFileBase
    {
        [Required]
        public string Data { get; set; }
    }
}