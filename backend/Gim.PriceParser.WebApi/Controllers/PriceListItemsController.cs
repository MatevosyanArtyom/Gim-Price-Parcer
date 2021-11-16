using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Bll.Services.PriceListItems;
using Gim.PriceParser.Bll.Services.PriceLists;
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
    public class PriceListItemsController : ApiControllerBase
    {
        private readonly IPriceListItemDao _dao;
        private readonly IMapper _mapper;
        private readonly IPriceListDao _priceListDao;
        private readonly IPriceListService _priceListService;
        private readonly IProductDao _productDao;
        private readonly IPriceListItemService _service;

        public PriceListItemsController(IPriceListItemDao dao, IPriceListDao priceListDao, IMapper mapper,
            IProductDao productDao, IPriceListItemService service, IPriceListService priceListService)
        {
            _dao = dao;
            _priceListDao = priceListDao;
            _mapper = mapper;
            _productDao = productDao;
            _service = service;
            _priceListService = priceListService;
        }

        [HttpGet]
        [Authorize]
        public async Task<GetAllResultDto<PriceListItemLookup>> GetManyIndexed([FromQuery] PriceListItemFilter filter,
            [FromQuery] SortParams sortParams, [FromQuery] int startIndex, [FromQuery] int stopIndex)
        {
            var docs = await _dao.GetManyAsync(filter, sortParams, startIndex, stopIndex);
            var docsDto = _mapper.Map<GetAllResultDto<PriceListItemLookup>>(docs);
            return docsDto;
        }

        [HttpGet]
        [Route("{id}/synonyms")]
        [Authorize]
        public async Task<List<ProductSynonymDto>> GetSynonymsMany([FromRoute] string id)
        {
            var item = await _dao.GetOneAsync(id);
            var ids = item.ProductSynonyms.Select(x => x.ProductId).ToList();
            var filter = new ProductFilter {Ids = ids};

            var products = await _productDao.GetManyIndexedAsync(filter);

            var idsDict = item.ProductSynonyms.ToDictionary(x => x.ProductId, x => x);

            var docs = products.Entities
                .Select(x => new ProductSynonym {Product = x, ProductId = x.Id, Score = idsDict[x.Id].Score})
                .OrderByDescending(x => x.Score)
                .ToList();

            var docsDto = _mapper.Map<List<ProductSynonymDto>>(docs);
            return docsDto;
        }

        [HttpPatch]
        [Route("/category-map-to-many/{priceListId}/{categoryId}/{level}")]
        [Authorize(Roles = KnownRoles.PriceListsEditSelf)]
        public async Task<ActionResult> SetCategoryMapToMany([FromRoute] string priceListId,
            [FromRoute] string categoryId, [FromRoute] int level, string categoryName)
        {
            if (!await CheckEditRights(priceListId))
            {
                return Forbid();
            }

            await _service.SetCategoryMapToManyAsync(priceListId, categoryId, level, categoryName);
            return Ok();
        }

        [HttpPatch]
        [Route("{id}/product/{productId}")]
        [Authorize(Roles = KnownRoles.PriceListsEditSelf)]
        public async Task<ActionResult> SetProductOne([FromRoute] string id, [FromRoute] string productId)
        {
            if (!await CheckItemEditRights(id))
            {
                return Forbid();
            }

            await _service.SetProductOneAsync(id, productId);
            return Ok();
        }

        [HttpPatch]
        [Route("{id}/skip-one/{skip}")]
        [Authorize(Roles = KnownRoles.PriceListsEditSelf)]
        public async Task<ActionResult> SetSkipOne([FromRoute] string id, [FromRoute] bool skip)
        {
            if (!await CheckItemEditRights(id))
            {
                return Forbid();
            }

            await _dao.SetSkipOneAsync(id, skip);
            var doc = await _dao.GetOneAsync(id);
            await _priceListService.UpdateStatuses(doc.PriceListId);

            return Ok();
        }

        [HttpPatch]
        [Route("skip-many")]
        [Authorize(Roles = KnownRoles.PriceListsEditSelf)]
        public async Task<ActionResult> SkipMany([FromQuery] PriceListItemFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.PriceListId) && !await CheckEditRights(filter.PriceListId))
            {
                return Forbid();
            }

            await _dao.SkipManyAsync(filter);
            await _priceListService.UpdateStatuses(filter.PriceListId);

            return Ok();
        }

        [HttpPatch]
        [Route("{id}/nameAction")]
        [Authorize(Roles = KnownRoles.PriceListsEditSelf)]
        public async Task<ActionResult> SetNameActionOne([FromRoute] string id, PriceListItemAction action)
        {
            if (!await CheckItemEditRights(id))
            {
                return Forbid();
            }

            await _service.SetNameActionOneAsync(id, action);

            return Ok();
        }

        private async Task<bool> CheckItemEditRights(string id)
        {
            var doc = await _dao.GetOneAsync(id);

            return await CheckEditRights(doc.PriceListId);
        }

        private async Task<bool> CheckEditRights(string priceListId)
        {
            var doc = await _priceListDao.GetOneAsync(priceListId);

            // Можно редактировать только "свои" прайс-листы, либо есть полные права
            return doc.AuthorId == CurrentUserId || HttpContext.User.IsInRole(KnownRoles.PriceListsFull);
        }
    }
}