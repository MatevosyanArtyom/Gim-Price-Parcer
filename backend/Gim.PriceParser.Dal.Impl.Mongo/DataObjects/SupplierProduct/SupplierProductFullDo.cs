using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Product;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SupplierProduct
{
    internal class SupplierProductFullDo : SupplierProductDo
    {
        public List<SupplierDo> Suppliers { get; set; }
        public List<ProductDo> Products { get; set; }
    }
}