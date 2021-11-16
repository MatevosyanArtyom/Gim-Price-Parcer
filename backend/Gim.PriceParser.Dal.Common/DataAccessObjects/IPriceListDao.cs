using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.Bll.Common.Sort;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    /// <summary>
    ///     Интерфейс для работы с файлами прайс-листов
    /// </summary>
    public interface IPriceListDao : IDaoBase<PriceList>
    {
        /// <summary>
        ///     Получает элементы постранично с фильтрацией
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="sort"></param>
        /// <param name="withData">Включать данные файла в результат</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <returns></returns>
        Task<GetAllResult<PriceList>> GetManyAsync(PriceListFilter filter, SortParams sort, bool withData = false,
            int page = 0, int pageSize = 0);

        /// <summary>
        ///     Получает один элемент колекции, исключая указанные поля
        /// </summary>
        /// <param name="id">Идентификатор элемента</param>
        /// <param name="excludeData">Исключить данные файла прайс-листа</param>
        /// <returns></returns>
        Task<PriceList> GetOneAsync(string id, bool excludeData);

        /// <summary>
        ///     Устанавливает статус для прайс-листа
        /// </summary>
        /// <param name="id">Идентификатор прайс-листа</param>
        /// <param name="status">Статус прайс-листа</param>
        /// <returns></returns>
        Task SetStatusAsync(string id, PriceListStatus status);
    }
}