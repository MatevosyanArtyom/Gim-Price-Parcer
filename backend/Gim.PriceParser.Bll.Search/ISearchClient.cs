using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.Products;

namespace Gim.PriceParser.Bll.Search
{
    /// <summary>
    ///     Интерфейс взаимодействия с системой гибкого поиска (ElasticSearch)
    /// </summary>
    public interface ISearchClient
    {
        /// <summary>
        ///     Добавляет список товаров
        /// </summary>
        /// <param name="products">Список товаров</param>
        /// <returns></returns>
        Task AddManyAsync(List<Product> products);

        /// <summary>
        ///     Выполняет поиск похожих товаров. Результаты устанавливаются в поле ProductSynonyms
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items);

        /// <summary>
        ///     Заменяет категорию во всех товарах, где она указана
        /// </summary>
        /// <param name="fromId">Идентификатор исходной категории</param>
        /// <param name="toId">Идентификатор конечной категонии</param>
        /// <returns></returns>
        Task SetCategoryManyAsync(string fromId, string toId);

        /// <summary>
        ///     Удаляет товары из поиска
        /// </summary>
        /// <returns></returns>
        Task DeleteManyAsync(ProductFilter filter = null);
    }
}