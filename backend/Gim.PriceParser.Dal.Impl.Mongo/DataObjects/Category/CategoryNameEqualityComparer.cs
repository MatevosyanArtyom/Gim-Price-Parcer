using System.Collections.Generic;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Category
{
    internal class CategoryNameEqualityComparer : IEqualityComparer<CategoryDo>
    {
        public bool Equals(CategoryDo x, CategoryDo y)
        {
            if (x != null && y != null)
            {
                return x.Name == y.Name;
            }

            return false;
        }

        public int GetHashCode(CategoryDo obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}