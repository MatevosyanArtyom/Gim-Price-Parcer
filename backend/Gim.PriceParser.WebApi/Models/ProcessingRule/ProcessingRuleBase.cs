using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;

namespace Gim.PriceParser.WebApi.Models.ProcessingRule
{
    public class ProcessingRuleBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Supplier { get; set; }

        [Required]
        public RulesSource RulesSource { get; set; }

        [Required]
        public string Code { get; set; }
    }
}