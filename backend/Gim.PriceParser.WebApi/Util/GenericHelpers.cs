using System;
using System.Collections.Generic;
using System.Linq;

namespace Gim.PriceParser.WebApi.Util
{
    public static class GenericHelpers
    {
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
            this IEnumerable<T> collection,
            Func<T, K> idSelector,
            Func<T, K> parentIdSelector,
            K rootId = default)
        {
            return collection.Where(c => parentIdSelector(c).Equals(rootId)).Select(c => new TreeItem<T>
            {
                Item = c,
                Children = collection.GenerateTree(idSelector, parentIdSelector, idSelector(c))
            });
        }
    }
}