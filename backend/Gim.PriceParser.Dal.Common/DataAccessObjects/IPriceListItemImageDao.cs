using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    /// <summary>
    ///     Интерфейс для работы c характеристиками (свойствами) строк прайс-листа
    /// </summary>
    public interface IPriceListItemImageDao : IDaoBase<PriceListItemImage>
    {
        Task<List<PriceListItemImage>> GetManyAsync(PriceListItemImageFilter filter);
        Task DeleteManyAsync(PriceListItemImageFilter filter);
    }
}