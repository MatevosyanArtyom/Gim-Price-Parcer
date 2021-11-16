using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Product;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Category
{
    internal class CategoryFullDo : CategoryDo
    {
        public IEnumerable<CategoryDo> Children { get; set; }
        public IEnumerable<CategoryDo> Parents { get; set; }
        public CategoryDo Parent { get; set; }
        public IEnumerable<ProductDo> Products { get; set; }
    }
}