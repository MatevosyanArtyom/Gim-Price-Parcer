using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Category;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Product;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SupplierProduct;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class ProductDao : DaoWithVersionsBase<Product, ProductDo, ProductVersionDo>, IProductDao
    {
        public const string CollectionName = "Products";
        private readonly ICategoryPropertyValueDao _valueDao;

        public ProductDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao,
            ICategoryPropertyValueDao valueDao) : base(mapper, db,
            sequenceCounterDao, CollectionName)
        {
            _valueDao = valueDao;
        }

        public async Task<GetAllResult<Product>> GetManyIndexedAsync(ProductFilter filter, SortParams sort = null,
            int startIndex = 0, int stopIndex = 0)
        {
            var filterDo = GimMapper.Map<FilterDefinition<ProductDo>>(filter);
            var sortDo = GimMapper.Map<SortDefinition<ProductDo>>(sort ?? new SortParams());

            var query = Col
                .Aggregate()
                .Match(filterDo)
                .Sort(sortDo)
                .Lookup<CategoryDo, ProductDo>(
                    CategoryDao.CollectionName,
                    nameof(ProductDo.CategoryId),
                    nameof(CategoryDo.Id),
                    nameof(ProductFullDo.Categories))
                .Lookup<SupplierDo, ProductDo>(
                    SupplierDao.CollectionName,
                    nameof(ProductDo.SupplierId),
                    nameof(SupplierDo.Id),
                    nameof(ProductFullDo.Suppliers))
                .Lookup<SupplierProductDo, ProductDo>(
                    SupplierProductDao.CollectionName,
                    nameof(ProductDo.Id),
                    nameof(SupplierProductDo.ProductId),
                    nameof(ProductFullDo.SupplierProducts))
                .Lookup<ImageDo, ProductDo>(
                    ImageDao.CollectionName,
                    nameof(ProductDo.Id),
                    nameof(ImageDo.ProductId),
                    nameof(ProductFullDo.Images));

            if (stopIndex > 0)
            {
                query = query
                    .Skip(startIndex)
                    .Limit(stopIndex - startIndex + 1);
            }

            var entitiesDo = await query
                .As<ProductFullDo>()
                .ToListAsync();

            var entities = GimMapper.Map<List<Product>>(entitiesDo);

            var valuesIds = GimMapper.Map<List<string>>(entitiesDo.SelectMany(x => x.Properties).ToList());
            var values = await _valueDao.GetManyAsync(new CategoryPropertyValueFilter {ValuesIds = valuesIds});

            entities.ForEach(e =>
            {
                for (var i = 0; i < e.Properties.Count; i++)
                {
                    var prop = e.Properties[i];
                    var value = values.FirstOrDefault(v => v.Id == prop.Id);
                    if (value != null)
                    {
                        e.Properties[i] = value;
                    }
                }
            });

            var result = new GetAllResult<Product>
            {
                Count = Col.CountDocuments(filterDo),
                Entities = entities
            };

            return result;
        }

        public async Task<List<string>> GetPropertiesAsync(string id)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filterDo = Builders<ProductDo>.Filter.Eq(x => x.Id, objId);
            var productDo = await Col.Find(filterDo).FirstOrDefaultAsync();
            var propertiesDo = productDo.Properties.Select(x => x);
            return GimMapper.Map<List<string>>(propertiesDo);
        }

        public async Task<long> CountAllAsync(ProductFilter filter)
        {
            var filterDo = GimMapper.Map<ProductFilter, FilterDefinition<ProductDo>>(filter);

            return await Col.CountDocumentsAsync(filterDo);
        }

        public async Task SetDescriptionOneAsync(string id, string description)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filterDo = Builders<ProductDo>.Filter.Eq(x => x.Id, objId);
            var updateDo = Builders<ProductDo>.Update.Set(x => x.Description, description);

            await Col.UpdateOneAsync(filterDo, updateDo);
        }

        public async Task SetPropertyValueOneAsync(string id, string oldId, string newId)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var objOldId = GimMapper.Map<ObjectId>(oldId);
            var objNewId = GimMapper.Map<ObjectId>(newId);

            var filterBuilder = Builders<ProductDo>.Filter;
            var filterDo = filterBuilder.And(filterBuilder.Eq(x => x.Id, objId),
                filterBuilder.Eq(nameof(ProductDo.Properties), objOldId));
            var updateDo = Builders<ProductDo>.Update.Set($"{nameof(ProductDo.Properties)}.$", objNewId);

            await Col.UpdateOneAsync(filterDo, updateDo);
        }

        public async Task SetCategoryManyAsync(string fromId, string toId)
        {
            var fromObjId = GimMapper.Map<ObjectId>(fromId);
            var toObjId = GimMapper.Map<ObjectId>(toId);
            await Col.UpdateManyAsync(x => x.CategoryId == fromObjId,
                Builders<ProductDo>.Update.Set(x => x.CategoryId, toObjId));
        }

        public override async Task<Product> GetOneAsync(string id)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filterDo = Builders<ProductDo>.Filter.Eq(x => x.Id, objId);

            var docDo = await Col
                .Aggregate()
                .Match(filterDo)
                .Lookup<CategoryDo, ProductDo>(
                    CategoryDao.CollectionName,
                    nameof(ProductDo.CategoryId),
                    nameof(CategoryDo.Id),
                    nameof(ProductFullDo.Categories))
                .Lookup<SupplierProductDo, ProductDo>(
                    SupplierProductDao.CollectionName,
                    nameof(ProductDo.Id),
                    nameof(SupplierProductDo.ProductId),
                    nameof(ProductFullDo.SupplierProducts))
                .As<ProductFullDo>()
                .FirstOrDefaultAsync();

            var doc = GimMapper.Map<Product>(docDo);
            return doc;
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
                    Id = GenerateNewObjectId(),
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
            var productsDo = await GetManyIndexedAsync(filter);

            var products = GimMapper.Map<List<Product>>(productsDo.Entities);
            var productsDict = products
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

        public async Task<List<PriceListItemMatched>> UpdateNamesAsync(List<PriceListItemMatched> items)
        {
            var requests = items
                .Where(item => item.NameAction == PriceListItemAction.ApplyNew)
                .Select(item =>
                {
                    var filterDo = Builders<ProductDo>.Filter.Eq(x => x.Id, GimMapper.Map<ObjectId>(item.ProductId));
                    var updateDo = Builders<ProductDo>.Update.Set(x => x.Name, item.ProductName);
                    return new UpdateOneModel<ProductDo>(filterDo, updateDo);
                })
                .ToList();

            if (requests.Any())
            {
                await Col.BulkWriteAsync(requests);
            }

            return items;
        }

        public async Task DeleteManyAsync(ProductFilter filter = null)
        {
            var filterDo = filter == null
                ? Builders<ProductDo>.Filter.Empty
                : GimMapper.Map<FilterDefinition<ProductDo>>(filter);
            await Col.DeleteManyAsync(filterDo);
        }

        public override async Task<GetAllResult<Product>> GetVersions(string id, int page, int pageSize)
        {
            Expression<Func<ProductVersionDo, bool>> filter = e => e.Entity.Id == GimMapper.Map<ObjectId>(id);

            var entities = await VersionsCol
                .Aggregate()
                .Match(filter)
                .Lookup<CategoryDo, ProductVersionFullDo>(
                    CategoryDao.CollectionName,
                    new ExpressionFieldDefinition<ProductVersionDo, ObjectId>(p => p.Entity.CategoryId),
                    nameof(CategoryDo.Id),
                    new ExpressionFieldDefinition<ProductVersionFullDo, IEnumerable<CategoryDo>>(p =>
                        p.Entity.Categories))
                .Lookup<SupplierDo, ProductVersionFullDo>(
                    SupplierDao.CollectionName,
                    new ExpressionFieldDefinition<ProductVersionFullDo, ObjectId>(p => p.Entity.SupplierId),
                    nameof(SupplierDo.Id),
                    new ExpressionFieldDefinition<ProductVersionFullDo, IEnumerable<SupplierDo>>(
                        p => p.Entity.Suppliers))
                .Skip(page * pageSize)
                .Limit(pageSize)
                .As<ProductVersionFullDo>()
                .ToListAsync();

            var result = new GetAllResult<Product>
            {
                Count = VersionsCol.CountDocuments(filter),
                Entities = GimMapper.Map<List<Product>>(entities)
            };

            return result;
        }
    }
}