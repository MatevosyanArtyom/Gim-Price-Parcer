namespace Gim.PriceParser.Bll.Common.Entities.SupplierProducts
{
    public class SupplierProductFilter
    {
        public string ProductId { get; set; }
        public long? SupplierSeqId { get; set; }
        public string SupplierName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
    }
}