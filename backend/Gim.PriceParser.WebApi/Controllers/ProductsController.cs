using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Categories;
using Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Bll.Services.Products;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.Product;
using Gim.PriceParser.WebApi.Models.ProductPropertyValue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gim.PriceParser.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ICategoryDao _categoryDao;
        private readonly IProductDao _dao;
        private readonly IImageDao _imageDao;
        private readonly IMapper _mapper;
        private readonly IProductService _service;
        private readonly ISupplierDao _supplierDao;
        private readonly ISupplierProductDao _supplierProductDao;
        private readonly ICategoryPropertyValueDao _valueDao;


        public ProductsController(ICategoryDao categoryDao, ICategoryPropertyValueDao valueDao, IImageDao imageDao,
            IProductDao dao, IProductService service, IMapper mapper, ISupplierDao supplierDao,
            ISupplierProductDao supplierProductDao)
        {
            _categoryDao = categoryDao;
            _valueDao = valueDao;
            _imageDao = imageDao;
            _dao = dao;
            _service = service;
            _mapper = mapper;
            _supplierDao = supplierDao;
            _supplierProductDao = supplierProductDao;
        }

        [HttpGet]
        [Route("indexed")]
        [Authorize(Roles = KnownRoles.ProductsRead)]
        public async Task<GetAllResultDto<ProductLookup>> GetManyIndexed([FromQuery] int startIndex,
            [FromQuery] int stopIndex, [FromQuery] SortParams sortParams, [FromQuery] ProductFilter filter)
        {
            var docs = await _dao.GetManyIndexedAsync(filter, sortParams, startIndex, stopIndex);
            var docsDto = _mapper.Map<GetAllResultDto<ProductLookup>>(docs);

            if (docs.Count > 0)
            {
                var categoryIds = docs.Entities
                    .SelectMany(x => new List<string>(x.Category.Ancestors) {x.CategoryId})
                    .Distinct()
                    .ToList();
                var categories = await _categoryDao.GetChildrenFlattenAsync(new CategoryFilter {Ids = categoryIds});
                var categoriesDict = categories.ToDictionary(x => x.Id, y => y);


                docsDto.Entities = docsDto.Entities.Select(x =>
                {
                    x.Category1 = ResolveCategoryName(categoriesDict, x.Category1);
                    x.Category2 = ResolveCategoryName(categoriesDict, x.Category2);
                    x.Category3 = ResolveCategoryName(categoriesDict, x.Category3);
                    x.Category4 = ResolveCategoryName(categoriesDict, x.Category4);
                    x.Category5 = ResolveCategoryName(categoriesDict, x.Category5);
                    return x;
                }).ToList();
            }

            return docsDto;
        }


        [HttpGet]
        [Route("count")]
        [Authorize]
        public async Task<long> CountAll([FromQuery] ProductFilter filter)
        {
            return await _dao.CountAllAsync(filter);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = KnownRoles.ProductsRead)]
        public async Task<ProductEdit> GetOne([FromRoute] string id)
        {
            var doc = await _dao.GetOneAsync(id);
            var docDto = _mapper.Map<ProductEdit>(doc);
            return docDto;
        }

        [HttpGet]
        [Route("{id}/properties")]
        [Authorize(Roles = KnownRoles.ProductsRead)]
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

        [HttpGet]
        [Route("lookup/{id}")]
        [Authorize(Roles = KnownRoles.ProductsRead)]
        public async Task<ProductLookup> GetLookupOne([FromRoute] string id)
        {
            var doc = await _dao.GetOneAsync(id);

            var categoryIds = new List<string>(doc.Category.Ancestors) {doc.CategoryId};
            var categories = await _categoryDao.GetChildrenFlattenAsync(new CategoryFilter {Ids = categoryIds});
            var categoriesDict = categories.ToDictionary(x => x.Id, y => y);

            var docDto = _mapper.Map<ProductLookup>(doc);
            docDto.Category1 = ResolveCategoryName(categoriesDict, docDto.Category1);
            docDto.Category2 = ResolveCategoryName(categoriesDict, docDto.Category2);
            docDto.Category3 = ResolveCategoryName(categoriesDict, docDto.Category3);
            docDto.Category4 = ResolveCategoryName(categoriesDict, docDto.Category4);
            docDto.Category5 = ResolveCategoryName(categoriesDict, docDto.Category5);
            return docDto;
        }

        [HttpPost]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task<ProductEdit> AddOne([FromBody] ProductAdd entity)
        {
            var doc = _mapper.Map<Product>(entity);
            doc = await _dao.AddOneAsync(doc);
            var docDto = _mapper.Map<ProductEdit>(doc);
            return docDto;
        }

        [HttpPost]
        [Route("many")]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task<List<ProductEdit>> AddManyRandom([FromForm] int count)
        {
            var categories = await _categoryDao.GetIds();
            var suppliers = await _supplierDao.GetIds();

            var faker = new Faker<Product>()
                .RuleFor(x => x.CategoryId, f => f.PickRandom(categories))
                .RuleFor(x => x.SupplierId, f => f.PickRandom(suppliers))
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Status, () => EntityStatus.Active)
                .RuleFor(x => x.Description, s => s.Lorem.Paragraph());
            var products = faker.Generate(count);
            var docs = await _dao.AddManyAsync(products);
            var docsDto = _mapper.Map<List<ProductEdit>>(docs);

            return docsDto;
        }

        [HttpPut]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task<ProductEdit> UpdateOne([FromBody] ProductEdit entity)
        {
            var doc = _mapper.Map<Product>(entity);
            doc = await _dao.UpdateOneAsync(doc);
            var docDto = _mapper.Map<ProductEdit>(doc);
            return docDto;
        }

        [HttpPatch]
        [Route("{id}/description")]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task SetDescriptionOne([FromRoute] string id, string description)
        {
            await _dao.SetDescriptionOneAsync(id, description);
        }

        [HttpPatch]
        [Route("{id}/property-value/{oldId}/{newId}")]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task SetPropertyValueOne([FromRoute] string id, [FromRoute] string oldId, [FromRoute] string newId)
        {
            await _dao.SetPropertyValueOneAsync(id, oldId, newId);
        }

        [HttpPatch]
        [Route("merge")]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task<IActionResult> MergeMany([FromBody] List<string> ids)
        {
            if (ids.Count < 2)
            {
                return BadRequest();
            }

            await _service.MergeManyAsync(ids);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task DeleteOne([FromQuery] string id)
        {
            await _dao.DeleteOneAsync(id);
        }

        [HttpDelete]
        [Route("many")]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task DeleteMany()
        {
            await _service.DeleteManyAsync();
            await _supplierProductDao.DeleteManyAsync();
            await _imageDao.DeleteManyAsync();
        }

        [HttpGet]
        [Route("{id}/versions")]
        [Authorize]
        public async Task<GetAllResultDto<ProductLookup>> GetVersions([FromRoute] string id, [FromQuery] int page,
            [FromQuery] int pageSize)
        {
            var docs = await _dao.GetVersions(id, page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<ProductLookup>>(docs);
            return docsDto;
        }

        [HttpPut]
        [Route("{id}/restore/{version}")]
        [Authorize(Roles = KnownRoles.ProductsFull)]
        public async Task<ProductEdit> RestoreVersion([FromRoute] string id, [FromRoute] string version)
        {
            var doc = await _dao.RestoreVersion(id, version);
            var docDto = _mapper.Map<ProductEdit>(doc);
            return docDto;
        }

        private static string ResolveCategoryName(IReadOnlyDictionary<string, Category> categories, string id)
        {
            return string.IsNullOrWhiteSpace(id) ? "" : categories[id].Name;
        }
    }
}