using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues;
using Gim.PriceParser.Bll.Common.Entities.SupplierProducts;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.Product;
using Gim.PriceParser.WebApi.Models.ProductPropertyValue;
using Gim.PriceParser.WebApi.Models.SupplierProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierProductsController : ControllerBase
    {
        private readonly ISupplierProductDao _dao;
        private readonly IMapper _mapper;
        private readonly ICategoryPropertyValueDao _valueDao;

        public SupplierProductsController(ISupplierProductDao dao, ICategoryPropertyValueDao valueDao, IMapper mapper)
        {
            _dao = dao;
            _valueDao = valueDao;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<GetAllResultDto<SupplierProductLookup>> GetMany([FromQuery] SupplierProductFilter filter,
            [FromQuery] int page, [FromQuery] int pageSize)
        {
            var docs = await _dao.GetManyAsync(filter, page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<SupplierProductLookup>>(docs);
            return docsDto;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<SupplierProductEdit> GetOne([FromRoute] string id)
        {
            var doc = await _dao.GetOneAsync(id);
            var docDto = _mapper.Map<SupplierProductEdit>(doc);
            return docDto;
        }

        [HttpGet]
        [Route("{id}/properties")]
        [Authorize]
        public async Task<ProductPropertiesModel> GetProperties([FromRoute] string id)
        {
            var ids = await _dao.GetPropertiesAsync(id);
            var values = await _valueDao.GetManyAsync(new CategoryPropertyValueFilter {ValuesIds = ids});
            var allValues = await _valueDao.GetManyAsync(new CategoryPropertyValueFilter
                {PropertiesIds = values.Select(x => x.PropertyId).Distinct().ToList()});

            var result = new ProductPropertiesModel
            {
                Values = _mapper.Map<List<ProductPropertyValueLookup>>(values),
                AllValues = _mapper.Map<List<ProductPropertyValueLookup>>(allValues)
            };
            return result;
        }

        [HttpPost]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task<SupplierProductEdit> AddOne([FromBody] SupplierProductAdd entity)
        {
            var doc = _mapper.Map<SupplierProduct>(entity);
            doc = await _dao.AddOneAsync(doc);
            var docDto = _mapper.Map<SupplierProductEdit>(doc);
            return docDto;
        }

        [HttpPut]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task<SupplierProductEdit> UpdateOne([FromBody] SupplierProductEdit entity)
        {
            var doc = _mapper.Map<SupplierProduct>(entity);
            doc = await _dao.UpdateOneAsync(doc);
            var docDto = _mapper.Map<SupplierProductEdit>(doc);
            return docDto;
        }

        [HttpDelete]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task DeleteOne([FromQuery] string id)
        {
            await _dao.DeleteOneAsync(id);
        }
    }
}