using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Categories;
using Gim.PriceParser.Bll.Services.Categories;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.WebApi.Auth;
using Gim.PriceParser.WebApi.Models;
using Gim.PriceParser.WebApi.Models.Category;
using Gim.PriceParser.WebApi.Models.GimFile;
using Gim.PriceParser.WebApi.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Gim.PriceParser.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController: ControllerBase
    {
        private readonly ICategoryDao _dao;
        private readonly IMapper _mapper;
        private readonly ICategoryPropertyDao _propertyDao;
        private readonly ICategoryService _service;
        private readonly ICategoryPropertyValueDao _valueDao;

        public CategoriesController(ICategoryDao dao, ICategoryService service, IMapper mapper,
            ICategoryPropertyDao propertyDao, ICategoryPropertyValueDao valueDao)
        {
            _dao = dao;
            _service = service;
            _mapper = mapper;
            _propertyDao = propertyDao;
            _valueDao = valueDao;
        }

        [HttpGet]
        [Authorize(Roles = KnownRoles.CategoriesRead)]
        public async Task<ActionResult<GetAllResultDto<CategoryLookup>>> GetMany()
        {
            var docs = await _dao.GetManyAsync();
            var docsDto = _mapper.Map<GetAllResultDto<CategoryLookup>>(docs);

            return docsDto;
        }

        [HttpGet]
        [Route("find")]
        [Authorize(Roles = KnownRoles.CategoriesRead)]
        public async Task<List<TreeItem<CategoryLookup>>> Find([FromQuery] string filter, [FromQuery] List<string> includeChildren)
        {
            var children = await _dao.FindOneAsync(filter);
            var childrenDto = _mapper.Map<List<TreeItem<CategoryLookup>>>(children);

            foreach (var child in childrenDto)
            {
                await AddChildren(child, includeChildren);
            }

            return childrenDto;
        }

        [HttpGet]
        [Route("children")]
        [Authorize(Roles = KnownRoles.CategoriesRead)]
        public async Task<List<TreeItem<CategoryLookup>>> GetChildren([FromQuery] string parentId,
            [FromQuery(Name = "includeChildren[]")] List<string> includeChildren)
        {
            var children = await _dao.GetChildrenAsync(parentId);
            var childrenDto = _mapper.Map<List<TreeItem<CategoryLookup>>>(children);

            foreach (var child in childrenDto)
            {
                await AddChildren(child, includeChildren);
            }

            return childrenDto;
        }

        [HttpGet]
        [Route("children-flatten")]
        [Authorize(Roles = KnownRoles.CategoriesRead)]
        public async Task<List<CategoryLookup>> GetChildrenFlatten([FromQuery(Name = "ids[]")] List<string> ids,
            [FromQuery(Name = "parents[]")] List<string> parents, [FromQuery] bool includeRoot)
        {
            parents = parents.Where(x => !string.IsNullOrWhiteSpace(x)).SelectMany(x => x.Split(',')).ToList();
            var children = await _service.GetChildrenFlattenAsync(ids, parents, includeRoot);
            var childrenDto = _mapper.Map<List<CategoryLookup>>(children);
            return childrenDto;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = KnownRoles.CategoriesRead)]
        public async Task<CategoryEdit> GetOne([FromRoute] string id)
        {
            var doc = await _dao.GetOneAsync(id);
            var docDto = _mapper.Map<CategoryEdit>(doc);
            return docDto;
        }

        [HttpPost]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task<ActionResult<CategoryEdit>> AddOne([FromBody] CategoryAdd entity)
        {
            var doc = _mapper.Map<Category>(entity);

            if (!string.IsNullOrEmpty(doc.ParentId))
            {
                var parent = await _dao.GetOneAsync(doc.ParentId);
                doc.Path += $"{parent.Path}/{doc.ParentId}";
                var depth = doc.Path.Split('/').Length;
                if (depth > 5)
                {
                    return BadRequest();
                }
            }

            doc = await _dao.AddOneAsync(doc);
            var docDto = _mapper.Map<CategoryEdit>(doc);
            return docDto;
        }

        [HttpPost]
        [Route("many")]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task<List<CategoryEdit>> AddManyRandom([FromForm] int count)
        {
            var rnd = new Random();
            var categories = new List<Category>();

            for (var i = 0; i < count; i++)
            {
                var faker = new Faker<Category>()
                    .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
                    .RuleFor(x => x.Name, f => string.Join(", ", f.Commerce.Categories(rnd.Next(2, 6))))
                    .RuleFor(x => x.Status, EntityStatus.Active);
                categories.Add(faker.Generate());
            }

            var docs = await _dao.AddManyAsync(categories);
            var docsDto = _mapper.Map<List<CategoryEdit>>(docs);

            return docsDto;
        }

        /// <summary>
        ///     Заполняет справочник категориями из яндекс-маркета
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("yandexMarket")]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task<List<CategoryEdit>> FromYandexMarket([FromBody] [Required] GimFileAdd file)
        {
            // EPPlus doesn't work with .xls files, .xlsx only
            // market file origin: http://download.cdn.yandex.net/market/market_categories.xls

            // Tuple format (CategoryName, ParentCategoryName or null)
            var marketCategories = new List<Tuple<string, string>>();

            var data = file.Data.Split(',')[1];
            var bytes = Convert.FromBase64String(data);

            using (var stream = new MemoryStream(bytes))
            {
                using (var ep = new ExcelPackage(stream))
                {
                    var sheets = ep.Workbook.Worksheets;
                    foreach (var sheet in sheets)
                    {
                        for (var i = 1; i <= sheet.Dimension.Rows; i++)
                        {
                            var value = sheet.Cells[i, 1].GetValue<string>();
                            var names = value.Split('/');

                            // First item in 'names' ignored
                            // Category row format:
                            // Все товары
                            // Все товары/Авто
                            // Все товары/Авто/Автомобильные инструменты
                            var start = names[0] == "Все товары" ? 1 : 0;
                            for (var j = start; j < names.Length; j++)
                            {
                                var categoryName = names[j];
                                var parentCategoryName = j > start ? names[j - 1] : null;
                                marketCategories.Add(new Tuple<string, string>(categoryName, parentCategoryName));
                            }
                        }
                    }
                }
            }

            marketCategories = marketCategories.Distinct().ToList();

            var categoriesDict = new Dictionary<string, Category>();
            var categories = new List<Category>();
            foreach (var (categoryName, parentCategoryName) in marketCategories)
            {
                var parentCategory = !string.IsNullOrWhiteSpace(parentCategoryName)
                    ? categoriesDict[parentCategoryName]
                    : null;

                var newId = _dao.GenerateNewObjectId();
                var category = new Category
                {
                    Id = newId,
                    Description = "Added from Yandex-Market",
                    Name = categoryName,
                    Path = parentCategory == null ? null : $"{parentCategory.Path}/{parentCategory.Id}",
                    ParentId = parentCategory?.Id,
                    Status = EntityStatus.Active
                };
                categories.Add(category);
                categoriesDict[category.Name] = category;
            }

            categories = await _dao.AddManyAsync(categories);

            var result = _mapper.Map<List<CategoryEdit>>(categories);
            return result;
        }

        [HttpPut]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task<CategoryEdit> UpdateOne([FromBody] CategoryEdit entity)
        {
            var doc = _mapper.Map<Category>(entity);
            doc = await _dao.UpdateOneAsync(doc);
            var docDto = _mapper.Map<CategoryEdit>(doc);
            return docDto;
        }

        [HttpPut]
        [Route("newParent")]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task<CategoryEdit> UpdateParent([FromBody] UpdateParentModel model)
        {
            var doc = await _dao.UpdateParentAsync(model.Id, model.NewParentId);
            var docDto = _mapper.Map<CategoryEdit>(doc);
            return docDto;
        }

        [HttpPatch]
        [Route("merge")]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task<CategoryEdit> MergeOne([FromBody] MergeCategoryModel model)
        {
            var doc = await _service.MergeOneAsync(model.FromId, model.ToId);
            var docDto = _mapper.Map<CategoryEdit>(doc);
            return docDto;
        }

        [HttpPatch]
        [Route("{id}/move")]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task<CategoryEdit> MoveOne([FromRoute] string id, [FromBody] MoveCategoryModel model)
        {
            var doc = await _dao.MoveOneAsync(id, model.AfterId);
            var docDto = _mapper.Map<CategoryEdit>(doc);
            return docDto;
        }

        [HttpPatch]
        [Route("{id}/mapings")]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task<CategoryEdit> UpdateMappings([FromRoute] string id,
            [FromBody] List<CategoryMappingItem> mappings)
        {
            var doc = await _dao.UpdateMappingsAsync(id, mappings);
            var docDto = _mapper.Map<CategoryEdit>(doc);
            return docDto;
        }

        [HttpDelete]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task<IActionResult> DeleteOne([FromQuery] string id)
        {
            try
            {
                await _service.DeleteOneAsync(id);
            }
            catch (InvalidOperationException)
            {
                return UnprocessableEntity();
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("many")]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task DeleteMany()
        {
            await Task.WhenAll(_dao.DeleteManyAsync(), _propertyDao.DeleteManyAsync(), _valueDao.DeleteManyAsync());
        }

        [HttpGet]
        [Route("versions")]
        [Authorize]
        public async Task<GetAllResultDto<CategoryLookup>> GetVersions([FromQuery] int page, [FromQuery] int pageSize)
        {
            var docs = await _dao.GetVersions(page, pageSize);
            var docsDto = _mapper.Map<GetAllResultDto<CategoryLookup>>(docs);
            return docsDto;
        }

        [HttpPut]
        [Route("restore/{version}")]
        [Authorize(Roles = KnownRoles.CategoriesFull)]
        public async Task RestoreVersion([FromRoute] string version)
        {
            await _dao.RestoreVersion(version);
        }

        private async Task AddChildren(TreeItem<CategoryLookup> item, ICollection<string> includeChildren)
        {
            if (!includeChildren.Contains(item.Item.Id))
            {
                return;
            }

            var children = await _dao.GetChildrenAsync(item.Item.Id);
            item.Children = _mapper.Map<List<TreeItem<CategoryLookup>>>(children);

            foreach (var child in item.Children)
            {
                await AddChildren(child, includeChildren);
            }
        }
    }
}
