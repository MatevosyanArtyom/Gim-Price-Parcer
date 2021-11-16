using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.User;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier
{
    internal class SupplierFullDo : SupplierDo
    {
        public List<UserDo> Users { get; set; }
    }
}