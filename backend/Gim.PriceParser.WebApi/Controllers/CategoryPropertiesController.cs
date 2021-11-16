using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.CategoryProperties;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.ProductProperty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class CategoryPropertiesController : ControllerBase
    {
        private readonly ICategoryPropertyDao _dao;
        private readonly IMapper _mapper;
        private readonly ICategoryPropertyValueDao _propertyValueDao;

        public CategoryPropertiesController(ICategoryPropertyDao dao, IMapper mapper,
            ICategoryPropertyValueDao propertyValueDao)
        {
            _dao = dao;
            _mapper = mapper;
            _propertyValueDao = propertyValueDao;
        }

        [HttpGet]
        [Authorize]
        public async Task<GetAllResultDto<ProductPropertyLookup>> GetMany([FromQuery] CategoryPropertyFilter filter)
        {
            var docs = await _dao.GetManyAsync(filter);
            var docsDto = _mapper.Map<GetAllResultDto<ProductPropertyLookup>>(docs);
            return docsDto;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ProductPropertyEdit> GetOne([FromRoute] string id)
        {
            var doc = await _dao.GetOneAsync(id);
            var docDto = _mapper.Map<ProductPropertyEdit>(doc);
            return docDto;
        }

        [HttpPost]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task<ProductPropertyEdit> AddOne([FromBody] ProductPropertyAdd entity)
        {
            var doc = _mapper.Map<CategoryProperty>(entity);
            doc = await _dao.AddOneAsync(doc);
            var docDto = _mapper.Map<ProductPropertyEdit>(doc);
            return docDto;
        }

        [HttpPut]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task<ProductPropertyEdit> UpdateOne([FromBody] ProductPropertyEdit entity)
        {
            var doc = _mapper.Map<CategoryProperty>(entity);
            doc = await _dao.UpdateOneAsync(doc);
            var docDto = _mapper.Map<ProductPropertyEdit>(doc);
            return docDto;
        }

        [HttpDelete]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task DeleteOne([FromQuery] string id)
        {
            await _dao.DeleteOneAsync(id);
            await _propertyValueDao.DeleteManyAsync(id);
        }
    }
}