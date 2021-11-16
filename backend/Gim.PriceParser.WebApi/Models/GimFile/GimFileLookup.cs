using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.GimFile
{
    public class GimFileLookup : GimFileBase
    {
        [Required]
        public string Id { get; set; }
    }
}