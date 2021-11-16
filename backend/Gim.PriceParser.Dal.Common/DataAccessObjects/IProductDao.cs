using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Gim.PriceParser.Bll.Common.Sort;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    /// <summary>
    ///     Интерфейс для работы с товарами
    /// </summary>
    public interface IProductDao : IDaoWithVersionsBase<Product>
    {
        Task<GetAllResult<Product>> GetManyIndexedAsync(ProductFilter filter, SortParams sort = null,
            int startIndex = 0, int stopIndex = 0);

        /// <summary>
        ///     Возвращает массив идентификаторов значений свойств номенклатуры
        /// </summary>
        /// <param name="id">Идентификатор номенклатуры</param>
        /// <returns></returns>
        Task<List<string>> GetPropertiesAsync(string id);

        Task<long> CountAllAsync(ProductFilter filter);

        /// <summary>
        ///     Устанавливает значение поля "Описание" для позиции номенклатуры
        /// </summary>
        /// <param name="id">Идентификатор номенклатуры</param>
        /// <param name="description">Значение поля</param>
        /// <returns></returns>
        Task SetDescriptionOneAsync(string id, string description);

        /// <summary>
        ///     Устанавливает значение характеристики для номенклатуры
        /// </summary>
        /// <param name="id">Идентификатор номенклатуры</param>
        /// <param name="oldId">Прежний идентификатор значения характеристики</param>
        /// <param name="newId">Новый идентификатор значения характеристики</param>
        /// <returns></returns>
        Task SetPropertyValueOneAsync(string id, string oldId, string newId);

        /// <summary>
        ///     Заменяет категорию во всех товарах, где она указана
        /// </summary>
        /// <param name="fromId">Идентификатор исходной категории</param>
        /// <param name="toId">Идентификатор конечной категонии</param>
        /// <returns></returns>
        Task SetCategoryManyAsync(string fromId, string toId);

        Task<List<PriceListItemMatched>> AddAbsentItemsAsync(List<PriceListItemMatched> items);
        Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items);
        Task<List<PriceListItemMatched>> UpdateNamesAsync(List<PriceListItemMatched> items);
        Task DeleteManyAsync(ProductFilter filter = null);
    }
}