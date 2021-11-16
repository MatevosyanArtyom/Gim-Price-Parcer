using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models.ProductPropertyValue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class CategoryPropertyValuesController : ControllerBase
    {
        private readonly ICategoryPropertyValueDao _dao;
        private readonly IMapper _mapper;

        public CategoryPropertyValuesController(ICategoryPropertyValueDao dao, IMapper mapper)
        {
            _dao = dao;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<ProductPropertyValueLookup>> GetMany([FromQuery] CategoryPropertyValueFilter filter)
        {
            var docs = await _dao.GetManyAsync(filter);
            var docsDto = _mapper.Map<List<ProductPropertyValueLookup>>(docs);
            return docsDto;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ProductPropertyValueEdit> GetOne([FromRoute] string id)
        {
            var doc = await _dao.GetOneAsync(id);
            var docDto = _mapper.Map<ProductPropertyValueEdit>(doc);
            return docDto;
        }

        [HttpPost]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task<ProductPropertyValueEdit> AddOne([FromBody] ProductPropertyValueAdd entity)
        {
            var doc = _mapper.Map<CategoryPropertyValue>(entity);
            doc = await _dao.AddOneAsync(doc);
            var docDto = _mapper.Map<ProductPropertyValueEdit>(doc);
            return docDto;
        }

        [HttpPut]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task<ProductPropertyValueEdit> UpdateOne([FromBody] ProductPropertyValueEdit entity)
        {
            var doc = _mapper.Map<CategoryPropertyValue>(entity);
            doc = await _dao.UpdateOneAsync(doc);
            var docDto = _mapper.Map<ProductPropertyValueEdit>(doc);
            return docDto;
        }

        [HttpDelete]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task DeleteOne([FromQuery] string id)
        {
            await _dao.DeleteOneAsync(id);
        }
    }
}