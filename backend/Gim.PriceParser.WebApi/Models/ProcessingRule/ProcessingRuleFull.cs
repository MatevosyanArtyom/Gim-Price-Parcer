using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.ProcessingRule
{
    public class ProcessingRuleFull : ProcessingRuleBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }

        public bool IsArchived { get; set; }
    }
}