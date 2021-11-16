using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Gim.PriceParser.Bll.Common.Sort;

namespace Gim.PriceParser.Bll.Services.Products
{
    public interface IProductService
    {
        Task<GetAllResult<Product>> GetManyIndexedAsync(ProductFilter filter, SortParams sort = null,
            int startIndex = 0, int stopIndex = 0);

        Task MergeManyAsync(List<string> ids);

        /// <summary>
        ///     Заменяет категорию во всех товарах, где она указана
        /// </summary>
        /// <param name="fromId">Идентификатор исходной категории</param>
        /// <param name="toId">Идентификатор конечной категонии</param>
        /// <returns></returns>
        Task SetCategoryManyAsync(string fromId, string toId);

        Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items);
        Task<List<PriceListItemMatched>> AddAbsentItemsAsync(List<PriceListItemMatched> items);
        Task DeleteManyAsync(ProductFilter filter = null);
    }
}