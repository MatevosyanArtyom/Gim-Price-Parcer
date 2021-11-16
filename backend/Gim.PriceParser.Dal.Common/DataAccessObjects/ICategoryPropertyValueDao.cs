using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    public interface ICategoryPropertyValueDao : IDaoBase<CategoryPropertyValue>
    {
        Task<List<CategoryPropertyValue>> GetManyAsync(CategoryPropertyValueFilter filter);
        Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items);

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<List<PriceListItemMatched>> AddAbsentItemsAsync(List<PriceListItemMatched> items);

        Task DeleteManyAsync();
        Task DeleteManyAsync(string propertyId);
    }
}