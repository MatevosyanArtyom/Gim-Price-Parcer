using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Suppliers;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class SuppliersController : ApiControllerBase
    {
        private readonly ISupplierDao _dao;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierDao dao, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _dao = dao;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        [HttpGet]
        [Authorize(Roles = KnownRoles.SuppliersReadSelf)]
        public async Task<GetAllResultDto<SupplierLookup>> GetMany([FromQuery] SupplierFilter filter,
            [FromQuery] SortParams sort, [FromQuery] int page, [FromQuery] int pageSize)
        {
            if (!_httpContext.HttpContext.User.IsInRole(KnownRoles.SuppliersRead))
            {
                filter.UserId = CurrentUserId;
            }

            var docs = await _dao.GetManyAsync(filter, sort, page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<SupplierLookup>>(docs);
            return docsDto;
        }

        [HttpGet]
        [Route("count")]
        [Authorize]
        public async Task<long> Count([FromQuery] SupplierFilter filter)
        {
            var count = await _dao.CountAsync(filter);
            return count;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = KnownRoles.SuppliersReadSelf)]
        public async Task<ActionResult<SupplierEdit>> GetOne([FromRoute] string id)
        {
            var doc = await _dao.GetOneAsync(id);
            
            if (!_httpContext.HttpContext.User.IsInRole(KnownRoles.SuppliersRead) && doc.UserId!= CurrentUserId)
            {
                return Forbid();
            }
            
            var docDto = _mapper.Map<SupplierEdit>(doc);
            return docDto;
        }

        [HttpPost]
        [Authorize(Roles = KnownRoles.SuppliersEditSelf)]
        public async Task<SupplierEdit> AddOne([FromBody] SupplierAdd entity)
        {
            var doc = _mapper.Map<Supplier>(entity);
            doc.UserId = CurrentUserId;
            doc = await _dao.AddOneAsync(doc);
            var docDto = _mapper.Map<SupplierEdit>(doc);
            return docDto;
        }

        [HttpPut]
        [Authorize(Roles = KnownRoles.SuppliersEditSelf)]
        public async Task<ActionResult<SupplierEdit>> UpdateOne([FromBody] SupplierEdit entity)
        {
            var doc = await _dao.GetOneAsync(entity.Id);

            // Пользователи без полных прав на поставщиков не могут менять статус
            // Пользователи без полных прав на поставщиков редактируют только "своих" поставщиков в статусе "Новый"
            if ((doc.UserId != CurrentUserId || doc.Status != entity.Status || doc.Status != EntityStatus.New) &&
                !_httpContext.HttpContext.User.IsInRole(KnownRoles.SuppliersFull))
            {
                return Forbid();
            }

            doc = _mapper.Map<Supplier>(entity);
            doc = await _dao.UpdateOneAsync(doc);
            var docDto = _mapper.Map<SupplierEdit>(doc);
            return docDto;
        }


        [HttpPatch]
        [Route("to-archive-one/{id}")]
        [Authorize(Roles = KnownRoles.SuppliersEditSelf)]
        public async Task<ActionResult> ToArchiveOne([FromRoute] string id)
        {
            if (!await CheckEditRights(id))
            {
                return Forbid();
            }

            await _dao.ToArchiveOneAsync(id);
            return Ok();
        }

        [HttpPatch]
        [Route("from-archive-one/{id}")]
        [Authorize(Roles = KnownRoles.SuppliersEditSelf)]
        public async Task<ActionResult> FromArchiveOne([FromRoute] string id)
        {
            if (!await CheckEditRights(id))
            {
                return Forbid();
            }

            await _dao.FromArchiveOneAsync(id);
            return Ok();
        }

        [HttpGet]
        [Route("{id}/versions")]
        [Authorize]
        public async Task<GetAllResultDto<EntityVersionDto<SupplierEdit>>> GetVersions([FromRoute] string id,
            [FromQuery] int page,
            [FromQuery] int pageSize)
        {
            var docs = await _dao.GetVersions(id, page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<EntityVersionDto<SupplierEdit>>>(docs);
            return docsDto;
        }

        [HttpPut]
        [Route("restore/{versionId}")]
        [Authorize(Roles = KnownRoles.SuppliersFull)]
        public async Task<SupplierEdit> RestoreVersion([FromRoute] string versionId)
        {
            var doc = await _dao.RestoreVersion(versionId);
            var docDto = _mapper.Map<SupplierEdit>(doc);
            return docDto;
        }

        private async Task<bool> CheckEditRights(string id)
        {
            var doc = await _dao.GetOneAsync(id);

            // Можно редактировать только "своих" в статусе "Новый", либо если есть полные права на поставщиков
            return doc.UserId == CurrentUserId && doc.Status == EntityStatus.New ||
                   _httpContext.HttpContext.User.IsInRole(KnownRoles.SuppliersFull);
        }
    }
}