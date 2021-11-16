using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;

namespace Gim.PriceParser.Bll.Services.PriceListItems
{
    /// <summary>
    ///     Класс бизнес-логики элементов прайс-листа
    /// </summary>
    public interface IPriceListItemService
    {
        /// <summary>
        ///     Устанавливает категорую, в которую будет добавлен аналог при утверждении прайс-листа
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
        ///     Устанавливает действие, которое будет выполнено с наименованием товара при утверждении прайс-листа
        /// </summary>
        /// <param name="id">Идентификатор строки прайс-листа</param>
        /// <param name="action">Действие</param>
        /// <returns></returns>
        Task SetNameActionOneAsync(string id, PriceListItemAction action);
    }
}