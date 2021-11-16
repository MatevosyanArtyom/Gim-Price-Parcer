using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.Manufacturer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ManufacturersController : ControllerBase
    {
        private readonly IManufacturerDao _dao;
        private readonly IMapper _mapper;

        public ManufacturersController(IManufacturerDao dao, IMapper mapper)
        {
            _dao = dao;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<GetAllResultDto<ManufacturerLookup>> GetMany([FromQuery] int page, [FromQuery] int pageSize)
        {
            var docs = await _dao.GetManyAsync(page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<ManufacturerLookup>>(docs);
            return docsDto;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ManufacturerEdit> GetOne([FromRoute] string id)
        {
            var doc = await _dao.GetOneAsync(id);
            var docDto = _mapper.Map<ManufacturerEdit>(doc);
            return docDto;
        }

        [HttpPost]
        [Authorize(Roles = KnownRoles.Moderator)]
        public async Task<ManufacturerEdit> AddOne([FromBody] ManufacturerAdd entity)
        {
            var doc = _mapper.Map<Manufacturer>(entity);
            doc = await _dao.AddOneAsync(doc);
            var docDto = _mapper.Map<ManufacturerEdit>(doc);
            return docDto;
        }

        [HttpPut]
        [Authorize(Roles = KnownRoles.Moderator)]
        public async Task<ManufacturerEdit> UpdateOne([FromBody] ManufacturerEdit entity)
        {
            var doc = _mapper.Map<Manufacturer>(entity);
            doc = await _dao.UpdateOneAsync(doc);
            var docDto = _mapper.Map<ManufacturerEdit>(doc);
            return docDto;
        }

        [HttpDelete]
        [Authorize(Roles = KnownRoles.Moderator)]
        public async Task DeleteOne([FromQuery] string id)
        {
            await _dao.DeleteOneAsync(id);
        }
    }
}