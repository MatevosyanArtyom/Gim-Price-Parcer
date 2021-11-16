using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Category;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SupplierProduct;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Product
{
    internal class ProductFullDo : ProductDo
    {
        public List<CategoryDo> Categories { get; set; }
        public List<SupplierDo> Suppliers { get; set; }
        public List<SupplierProductDo> SupplierProducts { get; set; }
        public List<ImageDo> Images { get; set; }
    }
}