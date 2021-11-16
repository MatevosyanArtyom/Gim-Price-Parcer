using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.ProcessingRules;
using Gim.PriceParser.Bll.Common.Entities.Suppliers;
using Gim.PriceParser.Bll.Common.Entities.UserRoles;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Bll.Services.Categories;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.Category;
using Gim.PriceParser.WebApi.Models.ProcessingRule;
using Gim.PriceParser.WebApi.Models.Supplier;
using Gim.PriceParser.WebApi.Models.UserRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    /// <summary>
    ///     Контроллер для получения вспомогательных данных (в формах). Права доступа не проверяются
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class LookupController : ApiControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IProcessingRuleDao _processingRuleDao;
        private readonly ISupplierDao _supplierDao;
        private readonly IUserRoleDao _userRoleDao;

        public LookupController(ICategoryService categoryService, ISupplierDao supplierDao, IMapper mapper,
            IProcessingRuleDao processingRuleDao, IUserRoleDao userRoleDao)
        {
            _categoryService = categoryService;
            _supplierDao = supplierDao;
            _mapper = mapper;
            _processingRuleDao = processingRuleDao;
            _userRoleDao = userRoleDao;
        }

        [HttpGet]
        [Route("categories")]
        public async Task<List<CategoryLookup>> CategoriesGetChildrenFlatten(
            [FromQuery(Name = "ids[]")] List<string> ids,
            [FromQuery(Name = "parents[]")] List<string> parents, [FromQuery] bool includeRoot)
        {
            var children = await _categoryService.GetChildrenFlattenAsync(ids, parents, includeRoot);
            var childrenDto = _mapper.Map<List<CategoryLookup>>(children);
            return childrenDto;
        }

        [HttpGet]
        [Route("suppliers-many")]
        public async Task<GetAllResultDto<SupplierShort>> SuppliersGetMany([FromQuery] SupplierFilter filter,
            [FromQuery] SortParams sort, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var docs = await _supplierDao.GetManyAsync(filter, sort, page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<SupplierShort>>(docs);
            return docsDto;
        }

        [HttpGet]
        [Route("suppliers/{id}")]
        public async Task<SupplierShort> SuppliersGetOne([FromRoute] string id)
        {
            var doc = await _supplierDao.GetOneAsync(id);
            var docDto = _mapper.Map<SupplierShort>(doc);
            return docDto;
        }

        [HttpGet]
        [Route("processing-rules")]
        public async Task<GetAllResultDto<ProcessingRuleLookup>> ProcessingRulesGetMany(
            [FromQuery] ProcessingRuleFilter filter,
            [FromQuery] SortParams sort, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var docs = await _processingRuleDao.GetManyAsync(filter, sort, page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<ProcessingRuleLookup>>(docs);
            return docsDto;
        }

        [HttpGet]
        [Route("user-roles")]
        public async Task<GetAllResultDto<UserRoleLookup>> UserRolesGetMany([FromQuery] GimUserRoleFilter filter,
            [FromQuery] SortParams sort, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var docs = await _userRoleDao.GetManyAsync(filter, sort, page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<UserRoleLookup>>(docs);
            return docsDto;
        }
    }
}