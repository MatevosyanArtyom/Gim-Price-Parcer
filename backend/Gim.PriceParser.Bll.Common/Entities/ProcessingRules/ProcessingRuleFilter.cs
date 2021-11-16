namespace Gim.PriceParser.Bll.Common.Entities.ProcessingRules
{
    public class ProcessingRuleFilter
    {
        public long? SeqId { get; set; }
        public string Name { get; set; }
        public string SupplierId { get; set; }
        public ArchivedFilter? ArchivedFilter { get; set; }
    }
}