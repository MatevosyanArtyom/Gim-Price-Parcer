using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;

namespace Gim.PriceParser.WebApi.Models.SchedulerTask
{
    public class CheckEmitPayload
    {
        [Required]
        public RulesSource RulesSource { get; set; }

        [Required]
        public string Script { get; set; }
    }
}
