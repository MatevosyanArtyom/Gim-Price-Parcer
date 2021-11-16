using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryProperty;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryPropertyValue;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceListItem
{
    internal class PriceListItemPropertyFullDo : PriceListItemPropertyDo
    {
        public List<CategoryPropertyDo> Properties { get; set; }
        public List<CategoryPropertyValueDo> Values { get; set; }
    }
}