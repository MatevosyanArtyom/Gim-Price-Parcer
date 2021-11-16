using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.Bll.Search;
using Gim.PriceParser.Dal.Common.DataAccessObjects;

namespace Gim.PriceParser.Bll.Services.PriceLists
{
    public class PriceListService : IPriceListService
    {
        private readonly IPriceListDao _dao;
        private readonly IPriceListItemDao _priceListItemDao;
        private readonly IPriceListItemPropertyDao _priceListItemPropertyDao;
        private readonly ISearchClient _searchClient;

        public PriceListService(IPriceListDao dao, IPriceListItemDao priceListItemDao,
            IPriceListItemPropertyDao priceListItemPropertyDao, ISearchClient searchClient)
        {
            _dao = dao;
            _priceListItemDao = priceListItemDao;
            _priceListItemPropertyDao = priceListItemPropertyDao;
            _searchClient = searchClient;
        }

        public async Task UpdateStatuses(string id)
        {
            var priceList = await _dao.GetOneAsync(id, true);

            // Ошибки артикула
            var filter = new PriceListItemFilter
            {
                PriceListId = id,
                UnprocessedCodeError = true
            };
            var items = await _priceListItemDao.GetManyAsync(filter, null, 0, 1);
            priceList.HasUnprocessedCodeErrors = items.Count > 0;

            // Ошибки наименования
            filter.UnprocessedCodeError = false;
            filter.UnprocessedNameErrors = true;
            items = await _priceListItemDao.GetManyAsync(filter, null, 0, 1);
            priceList.HasUnprocessedNameErrors = items.Count > 0;

            // Ошибки цены 1
            filter.UnprocessedNameErrors = false;
            filter.UnprocessedPriceError = true;
            items = await _priceListItemDao.GetManyAsync(filter, null, 0, 1);
            priceList.HasUnprocessedPriceErrors = items.Count > 0;

            // Любые ошибки
            filter.UnprocessedPriceError = false;
            filter.UnprocessedItemsOnly = true;
            items = await _priceListItemDao.GetManyAsync(filter, null, 0, 1);
            priceList.HasUnprocessedErrors = priceList.HasUnprocessedCodeErrors || priceList.HasUnprocessedNameErrors ||
                                             priceList.HasUnprocessedPriceErrors || items.Count > 0;

            // Получим идентификаторы всех свойств элементов этого прайс-листа
            var ids = await _priceListItemDao.GetIds(new PriceListItemFilter {PriceListId = id});
            var propsFilter = new PriceListItemPropertyFilter
            {
                PriceListItemsIds = ids,
                Status = PriceListItemStatus.Error
            };
            //Получим свойства с ошибками
            var props = await _priceListItemPropertyDao.GetManyIndexedAsync(propsFilter, 0, 1);
            priceList.HasPropertiesErrors = props.Count > 0;

            priceList.Status = !priceList.HasUnprocessedErrors ? PriceListStatus.Ready : PriceListStatus.Errors;

            await _dao.UpdateOneAsync(priceList);
        }

        public async Task SearchProducts(string id)
        {
            var filter = new PriceListItemFilter {PriceListId = id};
            var itemsResult = await _priceListItemDao.GetManyAsync(filter);
            var items = await _searchClient.MatchItemsAsync(itemsResult.Entities);
            await _priceListItemDao.SetSynonymsManyAsync(items);
        }
    }
}