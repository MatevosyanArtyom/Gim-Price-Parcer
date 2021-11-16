using System.Collections.Generic;

namespace Gim.PriceParser.Bll.Common.Entities.Products
{
    public class ProductFilter
    {
        public List<string> Ids { get; set; } = new List<string>();
        public long? SeqId { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string Category3 { get; set; }
        public string Category4 { get; set; }
        public string Category5 { get; set; }
        public string Name { get; set; }
        public List<string> Names { get; set; } = new List<string>();
        public EntityStatus? Status { get; set; }
    }
}