using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Sort;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    /// <summary>
    ///     Интерфейс для работы со строками прайс-листа
    /// </summary>
    public interface IPriceListItemDao : IDaoBase<PriceListItemMatched>
    {
        /// <summary>
        ///     Возвращает элементы постранично для указанного фильтра
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="sort"></param>
        /// <param name="startIndex"></param>
        /// <param name="stopIndex"></param>
        /// <returns></returns>
        Task<GetAllResult<PriceListItemMatched>> GetManyAsync(PriceListItemFilter filter, SortParams sort = null,
            int startIndex = 0, int stopIndex = 0);

        /// <summary>
        ///     Возвращает список идентификаторов строк прайс-листа для указанного фильтра
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <returns></returns>
        Task<List<string>> GetIds(PriceListItemFilter filter);

        /// <summary>
        ///     Удаляет все строки прайс-листа для выбранного прайс-листа
        /// </summary>
        /// <param name="priceListId">Идентификатор прайс-листа</param>
        /// <returns></returns>
        Task<List<string>> DeleteManyAsync(string priceListId);

        /// <summary>
        ///     Устанавливает признак пропуска строки при загрузке прайс-листа в БД
        /// </summary>
        /// <param name="id">Идентификатор строки прайс-листа</param>
        /// <param name="skip">Признак пропуска</param>
        /// <returns></returns>
        Task SetSkipOneAsync(string id, bool skip);

        /// <summary>
        ///     Устанавливает признак пропуска строки для строк с указанным фильтром
        /// </summary>
        /// <returns></returns>
        Task SkipManyAsync(PriceListItemFilter filter);

        /// <summary>
        ///     Устанавливает действие, которое будет выполнено с наименованием товара при загрузке прайс-листа в БД
        /// </summary>
        /// <param name="id">Идентификатор строки прайс-листа</param>
        /// <param name="action">Действие</param>
        /// <returns></returns>
        Task SetNameActionOneAsync(string id, PriceListItemAction action);

        /// <summary>
        ///     Устанавливает категорую, в которую будет добавлен аналог при загрузке прайс-листа в БД
        /// </summary>
        /// <param name="priceListId">Идентификатор прайс-листа</param>
        /// <param name="categoryId">Идентификатор категории</param>
        /// <param name="level">Уровень категории</param>
        /// <param name="categoryName">Наименование категории</param>
        /// <returns></returns>
        Task SetCategoryMapToManyAsync(string priceListId, string categoryId, int level, string categoryName);

        /// <summary>
        ///     Устанавливает товар, к которому будет применена выбранная строка
        ///     в случае, если товар был выбран из похожих (elastic)
        /// </summary>
        /// <param name="id">Идентификатор строки прайс-листа</param>
        /// <param name="productId">Идентификатор товара</param>
        /// <returns></returns>
        Task SetProductOneAsync(string id, string productId);

        /// <summary>
        ///     Устанавливает статус для выбранных элементов прайс-листа
        /// </summary>
        /// <param name="filter">Фильтр для отбора элементов</param>
        /// <param name="status">Статус для установки</param>
        /// <returns></returns>
        Task SetStatusManyAsync(PriceListItemFilter filter, PriceListItemStatus status);

        /// <summary>
        ///     Устанавливает список похожей номенклатуры для элементов прайс-листа (после повторного поиска в elastic)
        /// </summary>
        /// <param name="items">Список элементов прайс-листа</param>
        /// <returns></returns>
        Task SetSynonymsManyAsync(List<PriceListItemMatched> items);

        bool HasCategoryError(PriceListItemMatched item);
    }
}