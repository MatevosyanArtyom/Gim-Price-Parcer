using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Category;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryProperty
{
    internal class CategoryPropertyFullDo : CategoryPropertyDo
    {
        public List<CategoryDo> Categories { get; set; }
    }
}