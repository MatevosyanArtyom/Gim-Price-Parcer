using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;

namespace Gim.PriceParser.Processor
{
    /// <summary>
    ///     Основной класс парсинга (разбора) прайс-листов
    /// </summary>
    public interface IProcessorClient
    {
        Task ParsePriceListsAsync();

        Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemSource> items, string priceListId,
            string supplierId);

        /// <summary>
        ///     Загружает данные прайс-листа в БД
        /// </summary>
        /// <param name="id">Идентификатор прайс-листа</param>
        /// <returns></returns>
        Task CommitPriceListAsync(string id);

        /// <summary>
        ///     Загружает данные изображений по url
        /// </summary>
        /// <returns></returns>
        Task DownloadImages();
    }
}