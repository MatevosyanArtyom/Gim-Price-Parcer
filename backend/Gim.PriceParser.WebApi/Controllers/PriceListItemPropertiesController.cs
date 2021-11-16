using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.PriceListItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class PriceListItemPropertiesController : ControllerBase
    {
        private readonly IPriceListItemPropertyDao _dao;
        private readonly IMapper _mapper;
        private readonly IPriceListItemDao _priceListItemDao;
        private readonly IProductDao _productDao;
        private readonly ICategoryPropertyValueDao _valueDao;

        public PriceListItemPropertiesController(IPriceListItemPropertyDao dao, IPriceListItemDao priceListItemDao,
            IMapper mapper, IProductDao productDao, ICategoryPropertyValueDao valueDao)
        {
            _dao = dao;
            _priceListItemDao = priceListItemDao;
            _productDao = productDao;
            _valueDao = valueDao;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<PriceListItemProductPropertyLookup>> GetMany(
            [FromQuery] PriceListItemPropertyFilter filter, [FromQuery] string productId)
        {
            var docs = await _dao.GetManyIndexedAsync(filter);

            if (!string.IsNullOrWhiteSpace(productId))
            {
                var valuesIds = await _productDao.GetPropertiesAsync(productId);
                var values = await _valueDao.GetManyAsync(new CategoryPropertyValueFilter {ValuesIds = valuesIds});

                docs.Entities.ForEach(doc =>
                {
                    var value = values.FirstOrDefault(x => x.PropertyId == doc.PropertyId);
                    doc.ProductValue = value;
                    doc.ProductValueId = value?.Id;
                });
            }

            var docsDto = _mapper.Map<List<PriceListItemProductPropertyLookup>>(docs.Entities);
            return docsDto;
        }

        [HttpGet]
        [Route("indexed")]
        [Authorize]
        public async Task<GetAllResultDto<PriceListItemProductPropertyLookup>> GetManyIndexed(
            [FromQuery] string priceListId, [FromQuery] int startIndex, [FromQuery] int stopIndex)
        {
            var filter = new PriceListItemFilter
            {
                PriceListId = priceListId
            };
            var items = await _priceListItemDao.GetManyAsync(filter);
            var itemsIds = items.Entities.Select(item => item.Id).ToList();

            var propsFilter = new PriceListItemPropertyFilter
            {
                PriceListItemsIds = itemsIds
            };
            var props = await _dao.GetManyIndexedAsync(propsFilter, startIndex, stopIndex);

            var propsDto = _mapper.Map<GetAllResultDto<PriceListItemProductPropertyLookup>>(props);
            return propsDto;
        }

        [HttpPatch]
        [Route("{id}/action")]
        [Authorize(Roles = KnownRoles.PriceListsCreateProperties)]
        public async Task SetActionOne([FromRoute] string id, PriceListItemAction action)
        {
            await _dao.SetActionOneAsync(id, action);
        }

        [HttpPatch]
        [Route("action-many")]
        [Authorize(Roles = KnownRoles.PriceListsCreateProperties)]
        public async Task SetActionMany([FromBody] PriceListItemPropertySetActionModel model)
        {
            var itemsIds = await _priceListItemDao.GetIds(new PriceListItemFilter {PriceListId = model.PriceListId});

            var filter = new PriceListItemPropertyFilter {PriceListItemsIds = itemsIds, PropertyKey = model.PropertyKey};
            await _dao.SetActionManyAsync(filter, model.Action);
        }
    }
}