using System.Collections.Generic;

namespace Gim.PriceParser.Bll.Common.Entities.Categories
{
    public class CategoryFilter
    {
        public List<string> Ids { get; set; }
        public List<string> Parents { get; set; }
        public bool IncludeRoot { get; set; }
        public List<string> Names { get; set; }
    }
}