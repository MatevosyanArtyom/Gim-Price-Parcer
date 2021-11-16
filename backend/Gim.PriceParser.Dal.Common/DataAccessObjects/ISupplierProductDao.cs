using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.SupplierProducts;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    public interface ISupplierProductDao : IDaoBase<SupplierProduct>
    {
        Task<GetAllResult<SupplierProduct>> GetManyAsync(SupplierProductFilter filter, int page, int pageSize);

        /// <summary>
        ///     Возвращает массив идентификаторов значений свойств номенклатуры
        /// </summary>
        /// <param name="id">Идентификатор номенклатуры</param>
        /// <returns></returns>
        Task<List<string>> GetPropertiesAsync(string id);

        Task<List<PriceListItemMatched>> AddAbsentItemsAsync(List<PriceListItemMatched> items, string supplierId);
        Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items, string supplierId);
        Task<List<PriceListItemMatched>> UpdateItemsAsync(List<PriceListItemMatched> items, string supplierId);

        /// <summary>
        ///     Объединяет товары поставщика из разных позиций в одну
        /// </summary>
        /// <param name="productIds">Список номенклатуры для объединения. Товары поставщика объединяются в первый элемент списка</param>
        /// <returns></returns>
        Task MergeProducts(List<string> productIds);

        Task DeleteManyAsync();
    }
}