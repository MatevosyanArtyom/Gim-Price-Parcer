using Gim.PriceParser.Bll.Common.Entities.Products;

namespace Gim.PriceParser.Bll.Common.Entities.PriceListItem
{
    public class ProductSynonym
    {
        public Product Product { get; set; }
        public string ProductId { get; set; }
        public double Score { get; set; }
    }
}