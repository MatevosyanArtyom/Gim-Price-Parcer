using System.Threading.Tasks;

namespace Gim.PriceParser.Bll.Services.PriceLists
{
    public interface IPriceListService
    {
        /// <summary>
        ///     Вызывается после каждого действия с прайс-листом
        ///     Пропуск строк, маппинг категорий и т.д.
        /// </summary>
        /// <param name="id">Идентификатор прайс-листа</param>
        /// <returns></returns>
        Task UpdateStatuses(string id);

        /// <summary>
        ///     Повторить гибкий поиск номенклатуры (elastic)
        /// </summary>
        /// <param name="id">Идентификатор прайс-листа</param>
        /// <returns></returns>
        Task SearchProducts(string id);
    }
}