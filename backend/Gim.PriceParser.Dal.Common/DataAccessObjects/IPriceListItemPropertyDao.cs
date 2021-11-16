using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    /// <summary>
    ///     Интерфейс для работы c характеристиками (свойствами) строк прайс-листа
    /// </summary>
    public interface IPriceListItemPropertyDao : IDaoBase<PriceListItemPropertyMatched>
    {
        Task<GetAllResult<PriceListItemPropertyMatched>> GetManyIndexedAsync(PriceListItemPropertyFilter filter,
            int startIndex = 0, int stopIndex = 0);

        Task SetActionOneAsync(string id, PriceListItemAction action);
        Task SetActionManyAsync(PriceListItemPropertyFilter filter, PriceListItemAction action);
        Task DeleteManyAsync(PriceListItemPropertyFilter filter);
    }
}