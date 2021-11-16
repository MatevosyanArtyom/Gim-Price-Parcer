using System.Collections.Generic;

namespace Gim.PriceParser.Bll.Common.Entities.Categories
{
    public class CategoriesTreeResult
    {
        public Category Matched { get; set; }

        public List<Category> Children { get; set; } = new List<Category>();
    }
}