using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.CategoryProperties;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    public interface ICategoryPropertyDao : IDaoBase<CategoryProperty>
    {
        Task<GetAllResult<CategoryProperty>> GetManyAsync(CategoryPropertyFilter filter);
        Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items);
        Task<List<PriceListItemMatched>> AddAbsentItemsAsync(List<PriceListItemMatched> items);
        Task DeleteManyAsync();
    }
}