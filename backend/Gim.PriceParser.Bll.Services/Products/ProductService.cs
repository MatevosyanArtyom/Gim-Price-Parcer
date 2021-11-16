using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Bll.Search;
using Gim.PriceParser.Dal.Common.DataAccessObjects;

namespace Gim.PriceParser.Bll.Services.Products
{
    internal class ProductService : IProductService
    {
        private readonly IProductDao _dao;
        private readonly IImageDao _imageDao;
        private readonly ISearchClient _searchClient;
        private readonly ISupplierProductDao _supplierProductDao;

        public ProductService(IProductDao dao, ISearchClient searchClient, IImageDao imageDao,
            ISupplierProductDao supplierProductDao)
        {
            _dao = dao;
            _searchClient = searchClient;
            _imageDao = imageDao;
            _supplierProductDao = supplierProductDao;
        }

        public async Task<GetAllResult<Product>> GetManyIndexedAsync(ProductFilter filter, SortParams sort = null,
            int startIndex = 0, int stopIndex = 0)
        {
            return await _dao.GetManyIndexedAsync(filter, sort, startIndex, stopIndex);
        }

        public async Task MergeManyAsync(List<string> ids)
        {
            await _imageDao.MergeProducts(ids);
            await _supplierProductDao.MergeProducts(ids);

            var filter = new ProductFilter
            {
                Ids = ids.Skip(1).ToList()
            };
            await DeleteManyAsync(filter);
        }

        public async Task SetCategoryManyAsync(string fromId, string toId)
        {
            await _dao.SetCategoryManyAsync(fromId, toId);
            await _searchClient.SetCategoryManyAsync(fromId, toId);
        }

        public async Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items)
        {
            // ищем по номенклатуре только те позиции, что не были найдены по поставщикам
            var names = items
                .Where(x => string.IsNullOrWhiteSpace(x.ProductId) && !string.IsNullOrWhiteSpace(x.ProductName))
                .Select(x => x.ProductName)
                .Distinct()
                .ToList();

            var filter = new ProductFilter
            {
                Names = names
            };
            var products = await GetManyIndexedAsync(filter);

            var productsDict = products.Entities
                .GroupBy(x => x.Name, x => x)
                .ToDictionary(x => x.Key, x => x.First());

            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.ProductName) || !productsDict.ContainsKey(item.ProductName))
                {
                    continue;
                }

                var product = productsDict[item.ProductName];
                item.ProductId = product.Id;
                item.Product = product;
            }

            return items;
        }

        public async Task<List<PriceListItemMatched>> AddAbsentItemsAsync(List<PriceListItemMatched> items)
        {
            var newProducts = new List<Product>();

            items.ForEach(item =>
            {
                if (item.Skip || !string.IsNullOrWhiteSpace(item.ProductId))
                {
                    return;
                }

                var newProduct = new Product
                {
                    Id = _dao.GenerateNewObjectId(),
                    Name = item.ProductName,
                    Status = EntityStatus.New,
                    CategoryId = string.IsNullOrWhiteSpace(item.Category5Id)
                        ? string.IsNullOrWhiteSpace(item.Category4Id)
                            ? string.IsNullOrWhiteSpace(item.Category3Id)
                                ? string.IsNullOrWhiteSpace(item.Category2Id)
                                    ? string.IsNullOrWhiteSpace(item.Category1Id)
                                        ? null
                                        : item.Category1Id
                                    : item.Category2Id
                                : item.Category3Id
                            : item.Category4Id
                        : item.Category5Id,
                    Description = item.Description,
                    Properties = item.Properties
                        .Where(prop => !string.IsNullOrWhiteSpace(prop.PropertyId))
                        .Select(property => property.Value)
                        .ToList()
                };

                newProducts.Add(newProduct);

                item.Product = newProduct;
                item.ProductId = newProduct.Id;
            });

            if (newProducts.Any())
            {
                await AddManyAsync(newProducts);
            }

            return items;
        }

        public async Task DeleteManyAsync(ProductFilter filter = null)
        {
            await _dao.DeleteManyAsync(filter);
            await _searchClient.DeleteManyAsync(filter);
        }

        private async Task<List<Product>> AddManyAsync(List<Product> entities)
        {
            var docs = await _dao.AddManyAsync(entities);
            await _searchClient.AddManyAsync(entities);
            return docs;
        }
    }
}