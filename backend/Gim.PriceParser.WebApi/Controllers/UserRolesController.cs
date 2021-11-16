using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.UserRoles;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.UserRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleDao _dao;
        private readonly IMapper _mapper;

        public UserRolesController(IUserRoleDao dao, IMapper mapper)
        {
            _dao = dao;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = KnownRoles.UserRolesRead)]
        public async Task<GetAllResultDto<UserRoleLookup>> GetMany([FromQuery] GimUserRoleFilter filter,
            [FromQuery] SortParams sort, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var docs = await _dao.GetManyAsync(filter, sort, page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<UserRoleLookup>>(docs);
            return docsDto;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = KnownRoles.UserRolesRead)]
        public async Task<UserRoleEdit> GetOne([FromRoute] string id)
        {
            var doc = await _dao.GetOneAsync(id);
            var docDto = _mapper.Map<UserRoleEdit>(doc);
            return docDto;
        }

        [HttpPost]
        [Authorize(Roles = KnownRoles.UserRolesFull)]
        public async Task<UserRoleEdit> AddOne([FromBody] UserRoleAdd entity)
        {
            var doc = _mapper.Map<GimUserRole>(entity);
            doc = await _dao.AddOneAsync(doc);
            var docDto = _mapper.Map<UserRoleEdit>(doc);
            return docDto;
        }

        [HttpPut]
        [Authorize(Roles = KnownRoles.UserRolesFull)]
        public async Task<UserRoleEdit> UpdateOne([FromBody] UserRoleEdit entity)
        {
            var doc = _mapper.Map<GimUserRole>(entity);
            doc = await _dao.UpdateOneAsync(doc);
            var docDto = _mapper.Map<UserRoleEdit>(doc);
            return docDto;
        }

        [HttpPatch]
        [Route("to-archive-one/{id}")]
        [Authorize(Roles = KnownRoles.UserRolesFull)]
        public async Task ToArchiveOne([FromRoute] string id)
        {
            await _dao.ToArchiveOneAsync(id);
        }

        [HttpPatch]
        [Route("from-archive-one/{id}")]
        [Authorize(Roles = KnownRoles.UserRolesFull)]
        public async Task FromArchiveOne([FromRoute] string id)
        {
            await _dao.FromArchiveOneAsync(id);
        }
    }
}