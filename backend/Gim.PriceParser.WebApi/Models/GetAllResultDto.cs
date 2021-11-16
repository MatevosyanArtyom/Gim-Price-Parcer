using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models
{
    public class GetAllResultDto<T>
    {
        [Required]
        public long Count { get; set; }

        [Required]
        public List<T> Entities { get; set; }
    }
}