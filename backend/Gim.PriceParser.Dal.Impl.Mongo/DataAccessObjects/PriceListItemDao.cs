using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Sort;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Category;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceListItem;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Product;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SupplierProduct;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class PriceListItemDao : DaoBase<PriceListItemMatched, PriceListItemDo>, IPriceListItemDao
    {
        public const string CollectionName = "PriceListItems";

        public PriceListItemDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao) : base(mapper,
            db, sequenceCounterDao, CollectionName)
        {
        }

        public async Task<GetAllResult<PriceListItemMatched>> GetManyAsync(PriceListItemFilter filter,
            SortParams sort = null, int startIndex = 0, int stopIndex = 0)
        {
            var filterDo = GimMapper.Map<FilterDefinition<PriceListItemDo>>(filter);
            var sortDo = GimMapper.Map<SortDefinition<PriceListItemDo>>(sort ?? new SortParams());

            var query = Col
                .Aggregate()
                .Match(filterDo)
                .Sort(sortDo);

            if (stopIndex > 0)
            {
                query = query
                    .Skip(startIndex)
                    .Limit(stopIndex - startIndex + 1);
            }

            query = query.Lookup<SupplierProductDo, PriceListItemDo>(
                    SupplierProductDao.CollectionName,
                    nameof(PriceListItemDo.SupplierProductId),
                    nameof(SupplierProductDo.Id),
                    nameof(PriceListItemFullDo.SupplierProducts))
                .Lookup<CategoryDo, PriceListItemDo>(
                    CategoryDao.CollectionName,
                    nameof(PriceListItemDo.Category1Id),
                    nameof(CategoryDo.Id),
                    nameof(PriceListItemFullDo.Categories1))
                .Lookup<CategoryDo, PriceListItemDo>(
                    CategoryDao.CollectionName,
                    nameof(PriceListItemDo.Category2Id),
                    nameof(CategoryDo.Id),
                    nameof(PriceListItemFullDo.Categories2))
                .Lookup<CategoryDo, PriceListItemDo>(
                    CategoryDao.CollectionName,
                    nameof(PriceListItemDo.Category3Id),
                    nameof(CategoryDo.Id),
                    nameof(PriceListItemFullDo.Categories3))
                .Lookup<CategoryDo, PriceListItemDo>(
                    CategoryDao.CollectionName,
                    nameof(PriceListItemDo.Category4Id),
                    nameof(CategoryDo.Id),
                    nameof(PriceListItemFullDo.Categories4))
                .Lookup<CategoryDo, PriceListItemDo>(
                    CategoryDao.CollectionName,
                    nameof(PriceListItemDo.Category5Id),
                    nameof(CategoryDo.Id),
                    nameof(PriceListItemFullDo.Categories5))
                .Lookup<ProductDo, PriceListItemDo>(
                    ProductDao.CollectionName,
                    nameof(PriceListItemDo.ProductId),
                    nameof(ProductDo.Id),
                    nameof(PriceListItemFullDo.Products))
                .Lookup<PriceListItemImageDo, PriceListItemDo>(
                    PriceListItemImageDao.CollectionName,
                    nameof(PriceListItemDo.Id),
                    nameof(PriceListItemImageDo.PriceListItemId),
                    nameof(PriceListItemFullDo.Images))
                .Lookup<PriceListItemPropertyDo, PriceListItemDo>(
                    PriceListItemPropertyDao.CollectionName,
                    nameof(PriceListItemDo.Id),
                    nameof(PriceListItemPropertyDo.PriceListItemId),
                    nameof(PriceListItemFullDo.Properties));

            var entitiesDo = await query
                .As<PriceListItemFullDo>()
                .ToListAsync();

            var count = await Col.CountDocumentsAsync(filterDo);

            var result = new GetAllResult<PriceListItemMatched>
            {
                Count = count,
                Entities = GimMapper.Map<List<PriceListItemMatched>>(entitiesDo)
            };

            return result;
        }

        public async Task<List<string>> GetIds(PriceListItemFilter filter)
        {
            var filterDo = GimMapper.Map<FilterDefinition<PriceListItemDo>>(filter);
            var objIds = await Col
                .Find(filterDo)
                .Project(x => x.Id)
                .ToListAsync();

            var ids = GimMapper.Map<List<string>>(objIds);
            return ids;
        }

        public async Task<List<string>> DeleteManyAsync(string priceListId)
        {
            var priveListObjId = GimMapper.Map<ObjectId>(priceListId);
            var filterDo = Builders<PriceListItemDo>.Filter.Eq(x => x.PriceListId, priveListObjId);
            var ids = GimMapper.Map<List<string>>((await Col.Find(filterDo).ToListAsync()).Select(x => x.Id));
            await Col.DeleteManyAsync(filterDo);
            return ids;
        }

        public async Task SetSkipOneAsync(string id, bool skip)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filterDo = Builders<PriceListItemDo>.Filter.Eq(x => x.Id, objId);
            var updateDo = Builders<PriceListItemDo>.Update.Set(x => x.Skip, skip);

            await Col.UpdateOneAsync(filterDo, updateDo);
        }

        public async Task SkipManyAsync(PriceListItemFilter filter)
        {
            var filterDo = GimMapper.Map<FilterDefinition<PriceListItemDo>>(filter);
            var updateDo = Builders<PriceListItemDo>.Update.Set(x => x.Skip, true);

            await Col.UpdateManyAsync(filterDo, updateDo);
        }

        public async Task SetNameActionOneAsync(string id, PriceListItemAction action)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filter = Builders<PriceListItemDo>.Filter.Eq(x => x.Id, objId);
            var update = Builders<PriceListItemDo>.Update.Set(x => x.NameAction, action);

            await Col.UpdateOneAsync(filter, update);
        }

        public async Task SetCategoryMapToManyAsync(string priceListId, string categoryId, int level,
            string categoryName)
        {
            Expression<Func<PriceListItemDo, string>> nameField = x => x.Category1Name;
            Expression<Func<PriceListItemDo, ObjectId>> mapToField = x => x.MapTo1Id;
            Expression<Func<PriceListItemDo, PriceListItemCategoryAction>> categoryActionField = x => x.Category1Action;

            switch (level)
            {
                case 1:
                    nameField = x => x.Category1Name;
                    mapToField = x => x.MapTo1Id;
                    categoryActionField = x => x.Category1Action;
                    break;
                case 2:
                    nameField = x => x.Category2Name;
                    mapToField = x => x.MapTo2Id;
                    categoryActionField = x => x.Category2Action;
                    break;
                case 3:
                    nameField = x => x.Category3Name;
                    mapToField = x => x.MapTo3Id;
                    categoryActionField = x => x.Category3Action;
                    break;
                case 4:
                    nameField = x => x.Category4Name;
                    mapToField = x => x.MapTo4Id;
                    categoryActionField = x => x.Category4Action;
                    break;
                case 5:
                    nameField = x => x.Category5Name;
                    mapToField = x => x.MapTo5Id;
                    categoryActionField = x => x.Category5Action;
                    break;
            }

            var priceListObjId = GimMapper.Map<ObjectId>(priceListId);
            var categoryObjId = GimMapper.Map<ObjectId>(categoryId);

            var filterBuilder = Builders<PriceListItemDo>.Filter;
            var filterDo = filterBuilder.And(filterBuilder.Eq(x => x.PriceListId, priceListObjId),
                filterBuilder.Eq(new ExpressionFieldDefinition<PriceListItemDo, string>(nameField), categoryName));

            var mapToExpr = new ExpressionFieldDefinition<PriceListItemDo, ObjectId>(mapToField);
            var categoryActionExpr =
                new ExpressionFieldDefinition<PriceListItemDo, PriceListItemCategoryAction>(categoryActionField);
            var updateDo = Builders<PriceListItemDo>.Update
                .Set(mapToExpr, categoryObjId)
                .Set(categoryActionExpr, PriceListItemCategoryAction.MapTo);

            await Col.UpdateManyAsync(filterDo, updateDo);
        }

        public async Task SetProductOneAsync(string id, string productId)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var productObjId = GimMapper.Map<ObjectId>(productId);

            var filterDo = Builders<PriceListItemDo>.Filter.Eq(x => x.Id, objId);
            var updateDo = Builders<PriceListItemDo>.Update.Set(x => x.ProductId, productObjId);

            await Col.UpdateOneAsync(filterDo, updateDo);
        }

        public async Task SetStatusManyAsync(PriceListItemFilter filter, PriceListItemStatus status)
        {
            var filterDo = GimMapper.Map<FilterDefinition<PriceListItemDo>>(filter);
            var updateDo = Builders<PriceListItemDo>.Update.Set(x => x.Status, status);
            await Col.UpdateManyAsync(filterDo, updateDo);
        }

        public async Task SetSynonymsManyAsync(List<PriceListItemMatched> items)
        {
            var itemsDo = GimMapper.Map<List<PriceListItemDo>>(items);
            var requests = itemsDo.Select(itemDo =>
            {
                var filterDo = Builders<PriceListItemDo>.Filter.Eq(x => x.Id, itemDo.Id);
                var updateDo = Builders<PriceListItemDo>.Update.Set(x => x.ProductSynonyms, itemDo.ProductSynonyms);
                var model = new UpdateOneModel<PriceListItemDo>(filterDo, updateDo);
                return model;
            });
            await Col.BulkWriteAsync(requests);
        }

        public bool HasCategoryError(PriceListItemMatched item)
        {
            return string.IsNullOrWhiteSpace(item.Category1Id) && !string.IsNullOrWhiteSpace(item.Category1Name) ||
                   string.IsNullOrWhiteSpace(item.Category2Id) && !string.IsNullOrWhiteSpace(item.Category2Name) ||
                   string.IsNullOrWhiteSpace(item.Category3Id) && !string.IsNullOrWhiteSpace(item.Category3Name) ||
                   string.IsNullOrWhiteSpace(item.Category4Id) && !string.IsNullOrWhiteSpace(item.Category4Name) ||
                   string.IsNullOrWhiteSpace(item.Category5Id) && !string.IsNullOrWhiteSpace(item.Category5Name);
        }
    }
}