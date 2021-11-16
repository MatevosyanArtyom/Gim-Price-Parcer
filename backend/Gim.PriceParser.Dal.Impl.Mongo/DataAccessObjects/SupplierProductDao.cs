using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Gim.PriceParser.Bll.Common.Entities.SupplierProducts;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Product;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SupplierProduct;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class SupplierProductDao : DaoBase<SupplierProduct, SupplierProductDo>,
        ISupplierProductDao
    {
        public const string CollectionName = "SupplierProducts";
        private readonly IProductDao _productDao;

        public SupplierProductDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao,
            IProductDao productDao) : base(
            mapper, db, sequenceCounterDao, CollectionName)
        {
            _productDao = productDao;
        }

        public async Task<GetAllResult<SupplierProduct>> GetManyAsync(SupplierProductFilter filter, int page,
            int pageSize)
        {
            var filterDo = GimMapper.Map<FilterDefinition<SupplierProductDo>>(filter);

            var result = new GetAllResult<SupplierProduct>
            {
                Count = Col.CountDocuments(filterDo),
                Entities = await GetManyAsync(filterDo, null, page, pageSize)
            };

            return result;
        }

        public async Task<List<string>> GetPropertiesAsync(string id)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filterDo = Builders<SupplierProductDo>.Filter.Eq(x => x.Id, objId);
            var productDo = await Col.Find(filterDo).FirstOrDefaultAsync();
            var propertiesDo = productDo.Properties.Select(x => x);
            return GimMapper.Map<List<string>>(propertiesDo);
        }

        public async Task<List<PriceListItemMatched>> AddAbsentItemsAsync(List<PriceListItemMatched> items,
            string supplierId)
        {
            var newSupplierProducts = new List<SupplierProduct>();

            items.ForEach(item =>
            {
                // Элемент номенклатуры поставщика не редактируется.
                // Добавляется новый, если элементов нет
                // Добавляется новый, если данные изменились (цена, количество и т.д.)
                if (item.Skip || !string.IsNullOrWhiteSpace(item.SupplierProductId) && SupplierDataEquals(item))
                {
                    return;
                }

                var newSupplierProduct = new SupplierProduct
                {
                    Id = GenerateNewObjectId(),
                    Name = item.ProductName,
                    ProductId = item.ProductId,
                    SupplierId = supplierId,
                    Code = item.Code,
                    Price1 = item.Price1,
                    Price2 = item.Price2,
                    Price3 = item.Price3,
                    Quantity = item.Quantity,
                    Description = item.Description,
                    Properties = item.Properties
                        .Where(prop => !string.IsNullOrWhiteSpace(prop.PropertyId))
                        .Select(property => property.Value)
                        .ToList()
                };

                newSupplierProducts.Add(newSupplierProduct);

                item.SupplierProductId = newSupplierProduct.Id;
            });

            if (newSupplierProducts.Any())
            {
                await AddManyAsync(newSupplierProducts);
            }

            return items;
        }

        public async Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items,
            string supplierId)
        {
            var supplierObjId = GimMapper.Map<ObjectId>(supplierId);
            var codes = items.Where(x => !string.IsNullOrWhiteSpace(x.Code)).Select(x => x.Code).Distinct();
            var names = items.Where(x => !string.IsNullOrWhiteSpace(x.ProductName)).Select(x => x.ProductName)
                .Distinct();

            var filterDo = Builders<SupplierProductDo>.Filter.Or(
                Builders<SupplierProductDo>.Filter.In(x => x.Code, codes),
                Builders<SupplierProductDo>.Filter.In(x => x.Name, names)
            );
            filterDo = Builders<SupplierProductDo>.Filter.And(filterDo,
                Builders<SupplierProductDo>.Filter.Eq(x => x.SupplierId, supplierObjId)
            );
            var sortDo = Builders<SupplierProductDo>.Sort.Descending(x => x.Id);

            var ranges = await GetManyAsync(filterDo, sortDo);

            // TODO: Могут быть дубли?
            var rangesCodeDict = ranges.GroupBy(x => x.Code).ToDictionary(x => x.Key, x => x.First());
            var rangesNameDict = ranges.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.First());

            var productIds = ranges.Select(x => x.ProductId).Distinct().ToList();

            var products = productIds.Any()
                ? await _productDao.GetManyIndexedAsync(new ProductFilter {Ids = productIds})
                : new GetAllResult<Product>();

            var productsDict = products.Entities.ToDictionary(x => x.Id, x => x);

            items = items.Select(item =>
            {
                if (!string.IsNullOrWhiteSpace(item.Code) && rangesCodeDict.ContainsKey(item.Code))
                {
                    var productId = rangesCodeDict[item.Code].ProductId;
                    item.ProductId = productId;
                    item.Product = productsDict.ContainsKey(productId) ? productsDict[productId] : null;
                    item.SupplierProduct = rangesCodeDict[item.Code];
                    item.SupplierProductId = rangesCodeDict[item.Code].Id;
                    return item;
                }

                if (!string.IsNullOrWhiteSpace(item.ProductName) && rangesNameDict.ContainsKey(item.ProductName))
                {
                    var productId = rangesNameDict[item.ProductName].ProductId;
                    item.ProductId = productId;
                    item.Product = productsDict.ContainsKey(productId) ? productsDict[productId] : null;
                    item.SupplierProduct = rangesNameDict[item.ProductName];
                    item.SupplierProductId = rangesNameDict[item.ProductName].Id;
                }

                return item;
            }).ToList();

            return items;
        }

        public async Task<List<PriceListItemMatched>> UpdateItemsAsync(List<PriceListItemMatched> items,
            string supplierId)
        {
            var supplierProducts = items
                .Where(item =>
                    item.SupplierProduct == null ||
                    item.ProductName != item.SupplierProduct.Name ||
                    item.Price1 != item.SupplierProduct.Price1 ||
                    item.Price2 != item.SupplierProduct.Price2 ||
                    item.Price3 != item.SupplierProduct.Price3 ||
                    item.Quantity != item.SupplierProduct.Quantity)
                .Select(item => new SupplierProduct
                {
                    SupplierId = supplierId,
                    ProductId = item.ProductId,
                    Code = item.Code,
                    Name = item.ProductName,
                    Price1 = item.Price1,
                    Price2 = item.Price2,
                    Price3 = item.Price3,
                    Quantity = item.Quantity
                })
                .ToList();

            await AddManyAsync(supplierProducts);

            return items;
        }

        public async Task MergeProducts(List<string> productIds)
        {
            var productObjIds = GimMapper.Map<List<ObjectId>>(productIds);

            var filterDo = Builders<SupplierProductDo>.Filter.In(x => x.ProductId, productObjIds.Skip(1));
            var updateDo = Builders<SupplierProductDo>.Update.Set(x => x.ProductId, productObjIds.First());

            await Col.UpdateManyAsync(filterDo, updateDo);
        }

        public async Task DeleteManyAsync()
        {
            await Col.DeleteManyAsync(Builders<SupplierProductDo>.Filter.Empty);
        }

        private static bool SupplierDataEquals(PriceListItemMatched item)
        {
            return item.ProductName == item.SupplierProduct.Name && item.Price1 == item.SupplierProduct.Price1 &&
                   item.Price2 == item.SupplierProduct.Price2 && item.Price3 == item.SupplierProduct.Price3 &&
                   item.Quantity == item.SupplierProduct.Quantity;
        }

        private async Task<List<SupplierProduct>> GetManyAsync(FilterDefinition<SupplierProductDo> filterDo,
            SortDefinition<SupplierProductDo> sortDo = null, int page = 0, int pageSize = 0)
        {
            sortDo = sortDo ?? Builders<SupplierProductDo>.Sort.Ascending(x => x.Id);

            var query = Col
                .Aggregate()
                .Match(filterDo)
                .Sort(sortDo)
                .Lookup<SupplierDo, SupplierProductDo>(
                    SupplierDao.CollectionName,
                    nameof(SupplierProductDo.SupplierId),
                    nameof(SupplierDo.Id),
                    nameof(SupplierProductFullDo.Suppliers))
                .Lookup<ProductDo, SupplierProductDo>(
                    ProductDao.CollectionName,
                    nameof(SupplierProductDo.ProductId),
                    nameof(ProductDo.Id),
                    nameof(SupplierProductFullDo.Products));

            if (pageSize > 0)
            {
                query = query
                    .Skip(page * pageSize)
                    .Limit(pageSize);
            }

            var entities = await query
                .As<SupplierProductFullDo>()
                .ToListAsync();

            return GimMapper.Map<List<SupplierProduct>>(entities);
        }
    }
}