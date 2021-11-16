using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.PriceList
{
    public class PriceListBase
    {
        [Required]
        public string Supplier { get; set; }

        [Required]
        public string ProcessingRule { get; set; }

        //[Required]
        public string SchedulerTask { get; set; }

        //[Required]
        //public RulesSource RulesSource { get; set; }
    }
}