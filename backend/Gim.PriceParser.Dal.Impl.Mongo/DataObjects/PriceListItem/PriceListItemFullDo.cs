using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Category;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Product;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SupplierProduct;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceListItem
{
    internal class PriceListItemFullDo : PriceListItemDo
    {
        public List<ProductDo> Products { get; set; }
        public List<CategoryDo> Categories1 { get; set; }
        public List<CategoryDo> Categories2 { get; set; }
        public List<CategoryDo> Categories3 { get; set; }
        public List<CategoryDo> Categories4 { get; set; }
        public List<CategoryDo> Categories5 { get; set; }
        public List<SupplierProductDo> SupplierProducts { get; set; }
        public List<PriceListItemPropertyDo> Properties { get; set; }
        public List<PriceListItemImageDo> Images { get; set; }
    }
}