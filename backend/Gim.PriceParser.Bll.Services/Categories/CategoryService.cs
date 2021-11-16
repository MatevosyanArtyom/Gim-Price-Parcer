using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities.Categories;
using Gim.PriceParser.Bll.Services.Products;
using Gim.PriceParser.Dal.Common.DataAccessObjects;

namespace Gim.PriceParser.Bll.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDao _dao;
        private readonly IProductService _productService;

        public CategoryService(ICategoryDao dao, IProductService productService)
        {
            _dao = dao;
            _productService = productService;
        }

        public async Task<List<Category>> GetChildrenFlattenAsync(List<string> ids, List<string> parents,
            bool includeRoot)
        {
            var filter = new CategoryFilter
            {
                Ids = ids,
                Parents = parents,
                IncludeRoot = includeRoot
            };
            var docs = await _dao.GetChildrenFlattenAsync(filter);
            return docs;
        }

        public async Task<Category> MergeOneAsync(string fromId, string toId)
        {
            // replace category in products
            await _productService.SetCategoryManyAsync(fromId, toId);

            var doc = await _dao.MergeOneAsync(fromId, toId);
            return doc;
        }

        public async Task DeleteOneAsync(string id)
        {
            var doc = await _dao.GetOneAsync(id);

            if (doc.HasChildren || doc.ProductsCount > 0)
            {
                throw new InvalidOperationException();
            }

            await _dao.DeleteOneAsync(id);
        }
    }
}