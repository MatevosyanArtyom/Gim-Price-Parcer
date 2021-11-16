using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Services.PriceLists;
using Gim.PriceParser.Dal.Common.DataAccessObjects;

namespace Gim.PriceParser.Bll.Services.PriceListItems
{
    public class PriceListItemService : IPriceListItemService
    {
        private readonly IPriceListItemDao _dao;
        private readonly IPriceListService _priceListService;

        public PriceListItemService(IPriceListItemDao priceListItemDao, IPriceListService priceListService)
        {
            _dao = priceListItemDao;
            _priceListService = priceListService;
        }

        public async Task SetCategoryMapToManyAsync(string priceListId, string categoryId, int level,
            string categoryName)
        {
            await _dao.SetCategoryMapToManyAsync(priceListId, categoryId, level, categoryName);
            await SetFixedStatusManyAsync();

            await _priceListService.UpdateStatuses(priceListId);
        }

        public async Task SetProductOneAsync(string id, string productId)
        {
            await _dao.SetProductOneAsync(id, productId);
            await SetFixedStatusManyAsync();

            var doc = await _dao.GetOneAsync(id);
            await _priceListService.UpdateStatuses(doc.PriceListId);
        }

        public async Task SetNameActionOneAsync(string id, PriceListItemAction action)
        {
            await _dao.SetNameActionOneAsync(id, action);
            await SetFixedStatusManyAsync();

            var doc = await _dao.GetOneAsync(id);
            await _priceListService.UpdateStatuses(doc.PriceListId);
        }

        private async Task SetFixedStatusManyAsync()
        {
            var filter = new PriceListItemFilter
            {
                ProcessedItemsOnly = true
            };
            await _dao.SetStatusManyAsync(filter, PriceListItemStatus.Fixed);
        }
    }
}