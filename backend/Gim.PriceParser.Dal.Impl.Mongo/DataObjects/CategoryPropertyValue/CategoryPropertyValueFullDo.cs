using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryProperty;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryPropertyValue
{
    internal class CategoryPropertyValueFullDo : CategoryPropertyValueDo
    {
        public List<CategoryPropertyDo> Properties { get; set; }
    }
}