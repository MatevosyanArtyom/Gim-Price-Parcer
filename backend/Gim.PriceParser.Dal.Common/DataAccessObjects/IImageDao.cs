using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Images;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    public interface IImageDao : IDaoBase<GimImage>
    {
        /// <summary>
        ///     Получает элементы постранично
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="withData"></param>
        /// <returns></returns>
        Task<GetAllResult<GimImage>> GetManyAsync(GimImageFilter filter, int page, int pageSize, bool withData = false);

        /// <summary>
        ///     Получает главное изображение номенклатуры
        /// </summary>
        /// <param name="productId">Идентификатор номенклатуры</param>
        /// <returns></returns>
        Task<GimImage> GetMainAsync(string productId);

        /// <summary>
        ///     Заменяет список изображений
        /// </summary>
        /// <returns></returns>
        Task ReplaceManyAsync(List<GimImage> images);

        /// <summary>
        ///     Устанавливает признак главного изображения
        /// </summary>
        /// <param name="id">Идентификатор изображения</param>
        /// <returns></returns>
        Task SetMainAsync(string id);

        /// <summary>
        ///     Устанавливает признак публикации изображения
        /// </summary>
        /// <param name="id">Идентификатор изображения</param>
        /// <param name="isPublished"></param>
        /// <returns></returns>
        Task SetPublishedAsync(string id, bool isPublished);

        /// <summary>
        ///     Устанавливает статус для изображений
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="status">Статус для установки</param>
        Task SetStatusManyAsync(GimImageFilter filter, GimImageDownloadStatus status);

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items);

        Task<List<PriceListItemMatched>> AddAbsentItemsAsync(List<PriceListItemMatched> items);

        /// <summary>
        ///     Объединяет изображения номенклатуры из разных позиций в одну
        /// </summary>
        /// <param name="productIds">Список номенклатуры для объединения. Изображения объединяются в первый элемент списка</param>
        /// <returns></returns>
        Task MergeProducts(List<string> productIds);

        Task DeleteManyAsync();
    }
}