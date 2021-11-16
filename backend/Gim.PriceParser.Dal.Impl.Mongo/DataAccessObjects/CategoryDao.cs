using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Categories;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Category;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Product;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class CategoryDao : DaoWithVersionsBase<Category, CategoryDo, CategoryVersionDo>, ICategoryDao
    {
        public const string CollectionName = "Categories";
        private const int PositionStep = 1000;

        public CategoryDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao) : base(mapper, db,
            sequenceCounterDao, CollectionName)
        {
            Col.Indexes.CreateOneAsync(
                new CreateIndexModel<CategoryDo>(Builders<CategoryDo>.IndexKeys.Text(x => x.Path)));
        }

        public override async Task<Category> AddOneAsync(Category entity)
        {
            var pos = await GetMaxPosition();
            entity.Position = pos + PositionStep;
            return await base.AddOneAsync(entity);
        }

        public async Task<Category> UpdateMappingsAsync(string id, List<CategoryMappingItem> mappings)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filter = Builders<CategoryDo>.Filter.Eq(x => x.Id, objId);
            var update = Builders<CategoryDo>.Update.Set(x => x.Mappings, mappings);
            await UpdateOneAsync(filter, update);

            return await GetOneAsync(id);
        }

        public async Task<Category> MergeOneAsync(string fromId, string toId)
        {
            var fromObjId = GimMapper.Map<ObjectId>(fromId);
            var toObjId = GimMapper.Map<ObjectId>(toId);

            var newCategory = await Col.Find(x => x.Id == toObjId).FirstOrDefaultAsync();

            // replace parent and path in categories
            var requests = new List<WriteModel<CategoryDo>>();
            var cursor = await Col.Find(x => x.Path.Contains(fromId)).ToCursorAsync();
            while (await cursor.MoveNextAsync())
            {
                foreach (var item in cursor.Current)
                {
                    var filter = Builders<CategoryDo>.Filter.Eq(x => x.Id, item.Id);
                    var pathTail = Regex.Split(item.Path, fromId)[1];
                    pathTail = pathTail.Any() ? pathTail : "";
                    var newPath = $"{newCategory.Path}/{newCategory.Id}{pathTail}";
                    var update = Builders<CategoryDo>.Update
                        .Set(x => x.ParentId, toObjId)
                        .Set(x => x.Path, newPath);
                    requests.Add(new UpdateOneModel<CategoryDo>(filter, update));
                }
            }

            if (requests.Any())
            {
                await Col.BulkWriteAsync(requests);
            }

            // remove old category
            await Col.DeleteOneAsync(x => x.Id == fromObjId);

            // return new category
            var categoryDo = await Col.Find(x => x.Id == toObjId).FirstOrDefaultAsync();
            return GimMapper.Map<Category>(categoryDo);
        }

        public async Task<Category> MoveOneAsync(string id, string afterId)
        {
            int newPosition;
            if (string.IsNullOrWhiteSpace(afterId))
            {
                var docBefore = await Col
                    .Aggregate()
                    .Sort(Builders<CategoryDo>.Sort.Ascending(x => x.Position))
                    .FirstOrDefaultAsync();
                newPosition = docBefore.Position / 2;
            }
            else
            {
                var docAfter = await GetOneAsync(afterId);
                var docBefore = await Col
                    .Aggregate()
                    .Match(Builders<CategoryDo>.Filter.Gt(x => x.Position, docAfter.Position))
                    .Sort(Builders<CategoryDo>.Sort.Ascending(x => x.Position))
                    .FirstOrDefaultAsync();

                newPosition = docBefore == null
                    ? docAfter.Position + PositionStep
                    : (docBefore.Position + docAfter.Position) / 2;
            }

            var objId = GimMapper.Map<ObjectId>(id);
            var filter = Builders<CategoryDo>.Filter.Eq(x => x.Id, objId);
            var update = Builders<CategoryDo>.Update.Set(x => x.Position, newPosition);

            await Col.UpdateOneAsync(filter, update);

            return await GetOneAsync(id);
        }

        public override async Task<Category> GetOneAsync(string id)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filterDo = Builders<CategoryDo>.Filter.Eq(x => x.Id, objId);

            var af = Col
                .Aggregate()
                .Match(filterDo)
                .As<CategoryFullDo>();

            af = AddLookups(af);

            var docDo = await af.FirstOrDefaultAsync();
            var doc = GimMapper.Map<Category>(docDo);
            return doc;
        }

        public async Task<List<Category>> FindOneAsync(string filter)
        {
            var filterBuilder = Builders<CategoryDo>.Filter;
            var filterDo = string.IsNullOrWhiteSpace(filter)
                ? filterBuilder.Empty
                : filterBuilder.Regex(x => x.Name, $"/.*{filter}.*/i");


            // find first matching element
            var af = Col
                .Aggregate()
                .Match(filterDo)
                .Sort(Builders<CategoryDo>.Sort.Ascending(x => x.Name))
                .Limit(1)
                .As<CategoryFullDo>();

            af = AddLookups(af);

            var entitiesDo = await af.ToListAsync();

            var entities = GimMapper.Map<List<Category>>(entitiesDo);
            return entities;
        }

        public async Task<List<PriceListItemMatched>> AddAbsentItemsAsync(List<PriceListItemMatched> items)
        {
            // Сначала обработаем замаппленные категории
            var newMappings = new List<KeyValuePair<string, string>>();
            foreach (var item in items)
            {
                var pairs = new List<KeyValuePair<string, string>>();

                if (!string.IsNullOrWhiteSpace(item.MapTo1Id))
                {
                    pairs.Add(new KeyValuePair<string, string>(item.MapTo1Id, item.Category1Name));
                    item.Category1Id = item.MapTo1Id;
                }

                if (!string.IsNullOrWhiteSpace(item.MapTo2Id))
                {
                    pairs.Add(new KeyValuePair<string, string>(item.MapTo2Id, item.Category2Name));
                    item.Category2Id = item.MapTo2Id;
                }

                if (!string.IsNullOrWhiteSpace(item.MapTo3Id))
                {
                    pairs.Add(new KeyValuePair<string, string>(item.MapTo3Id, item.Category3Name));
                    item.Category3Id = item.MapTo3Id;
                }

                if (!string.IsNullOrWhiteSpace(item.MapTo4Id))
                {
                    pairs.Add(new KeyValuePair<string, string>(item.MapTo4Id, item.Category4Name));
                    item.Category4Id = item.MapTo4Id;
                }

                if (!string.IsNullOrWhiteSpace(item.MapTo5Id))
                {
                    pairs.Add(new KeyValuePair<string, string>(item.MapTo5Id, item.Category5Name));
                    item.Category5Id = item.MapTo5Id;
                }

                newMappings.AddRange(pairs);
            }

            var requests = newMappings
                .Distinct()
                .Select(m =>
                {
                    var objId = GimMapper.Map<ObjectId>(m.Key);
                    var filter = Builders<CategoryDo>.Filter.Eq(x => x.Id, objId);
                    var update = Builders<CategoryDo>.Update.Push(x => x.Mappings,
                        new CategoryMappingItem {Name = m.Value, CreatedDate = DateTime.Now});
                    return new UpdateOneModel<CategoryDo>(filter, update);
                })
                .ToList();

            if (requests.Any())
            {
                await Col.BulkWriteAsync(requests);
            }

            var newCategories = new List<Category>();

            // 1
            var names1 = items
                // Если категория не сопоставлена, и не установлен маппинг, нужно ее создать
                .Where(x => string.IsNullOrWhiteSpace(x.Category1Id) && !string.IsNullOrWhiteSpace(x.Category1Name) &&
                            string.IsNullOrWhiteSpace(x.MapTo1Id))
                // Если категория ниже в дереве сопоставлена, не нужно создавать текущую
                .Where(x => string.IsNullOrWhiteSpace(x.Category2Id) && string.IsNullOrWhiteSpace(x.Category3Id) &&
                            string.IsNullOrWhiteSpace(x.Category4Id) && string.IsNullOrWhiteSpace(x.Category5Id))
                .ToLookup(x => x.Category1Name, x => x);

            foreach (var grouping in names1)
            {
                var newCategory = new Category
                {
                    Id = GenerateNewObjectId(),
                    Name = grouping.Key
                };

                newCategories.Add(newCategory);

                foreach (var item in grouping)
                {
                    item.Category1Id = newCategory.Id;
                    item.Category1 = newCategory;
                }
            }

            // 2
            var names2 = items
                .Where(x => string.IsNullOrWhiteSpace(x.Category2Id) && !string.IsNullOrWhiteSpace(x.Category2Name) &&
                            string.IsNullOrWhiteSpace(x.MapTo2Id))
                .Where(x => string.IsNullOrWhiteSpace(x.Category3Id) && string.IsNullOrWhiteSpace(x.Category4Id) &&
                            string.IsNullOrWhiteSpace(x.Category5Id))
                .ToLookup(x => x.Category2Name, x => x);

            foreach (var grouping in names2)
            {
                var anyItem = grouping.First();
                var newCategory = new Category
                {
                    Id = GenerateNewObjectId(),
                    Name = grouping.Key,
                    ParentId = anyItem.Category1Id,
                    Path = $"/{anyItem.Category1Id}"
                };

                newCategories.Add(newCategory);

                foreach (var item in grouping)
                {
                    item.Category2Id = newCategory.Id;
                    item.Category2 = newCategory;
                }
            }

            // 3
            var names3 = items
                .Where(x => string.IsNullOrWhiteSpace(x.Category3Id) && !string.IsNullOrWhiteSpace(x.Category3Name) &&
                            string.IsNullOrWhiteSpace(x.MapTo3Id))
                .Where(x => string.IsNullOrWhiteSpace(x.Category4Id) && string.IsNullOrWhiteSpace(x.Category5Id))
                .ToLookup(x => x.Category3Name, x => x);

            foreach (var grouping in names3)
            {
                var anyItem = grouping.First();
                var newCategory = new Category
                {
                    Id = GenerateNewObjectId(),
                    Name = grouping.Key,
                    ParentId = anyItem.Category2Id,
                    Path = $"{anyItem.Category2.Path}/{anyItem.Category2Id}"
                };

                newCategories.Add(newCategory);

                foreach (var item in grouping)
                {
                    item.Category3Id = newCategory.Id;
                    item.Category3 = newCategory;
                }
            }

            // 4
            var names4 = items
                .Where(x => string.IsNullOrWhiteSpace(x.Category4Id) && !string.IsNullOrWhiteSpace(x.Category4Name) &&
                            string.IsNullOrWhiteSpace(x.MapTo4Id))
                .Where(x => string.IsNullOrWhiteSpace(x.Category5Id))
                .ToLookup(x => x.Category4Name, x => x);

            foreach (var grouping in names4)
            {
                var anyItem = grouping.First();
                var newCategory = new Category
                {
                    Id = GenerateNewObjectId(),
                    Name = grouping.Key,
                    ParentId = anyItem.Category3Id,
                    Path = $"{anyItem.Category3.Path}/{anyItem.Category3Id}"
                };

                newCategories.Add(newCategory);

                foreach (var item in grouping)
                {
                    item.Category4Id = newCategory.Id;
                    item.Category4 = newCategory;
                }
            }

            // 5
            var names5 = items
                .Where(x => string.IsNullOrWhiteSpace(x.Category5Id) && !string.IsNullOrWhiteSpace(x.Category5Name) &&
                            string.IsNullOrWhiteSpace(x.MapTo5Id))
                .ToLookup(x => x.Category5Name, x => x);

            foreach (var grouping in names5)
            {
                var anyItem = grouping.First();
                var newCategory = new Category
                {
                    Id = GenerateNewObjectId(),
                    Name = grouping.Key,
                    ParentId = anyItem.Category4Id,
                    Path = $"{anyItem.Category4.Path}/{anyItem.Category4Id}"
                };

                newCategories.Add(newCategory);

                foreach (var item in grouping)
                {
                    item.Category5Id = newCategory.Id;
                    item.Category5 = newCategory;
                }
            }

            if (newCategories.Any())
            {
                await AddManyAsync(newCategories);
            }

            return items;
        }

        public async Task<List<Category>> GetChildrenAsync(string parentId)
        {
            var parentObjId = GimMapper.Map<ObjectId>(parentId);

            var af = Col
                .Aggregate()
                .Match(Builders<CategoryDo>.Filter.Eq(x => x.ParentId, parentObjId))
                .As<CategoryFullDo>();

            af = AddLookups(af);

            var entitiesDo = await af.ToListAsync();

            return GimMapper.Map<List<Category>>(entitiesDo);
        }

        public async Task<List<Category>> GetChildrenFlattenAsync(CategoryFilter filter)
        {
            var filterDo = GimMapper.Map<FilterDefinition<CategoryDo>>(filter);

            var entitiesDo = await Col
                .Aggregate()
                .Match(filterDo)
                .Sort(Builders<CategoryDo>.Sort.Ascending(x => x.Position))
                .Lookup<CategoryDo, CategoryFullDo>(
                    CollectionName,
                    nameof(CategoryDo.Id),
                    nameof(CategoryDo.ParentId),
                    nameof(CategoryFullDo.Children))
                .Lookup<CategoryFullDo, CategoryFullDo>(
                    ProductDao.CollectionName,
                    nameof(CategoryDo.Id),
                    nameof(ProductDo.CategoryId),
                    nameof(CategoryFullDo.Products))
                .As<CategoryFullDo>()
                .ToListAsync();

            return GimMapper.Map<List<Category>>(entitiesDo);
        }

        public async Task RestoreVersion(string version)
        {
            var versionObjId = GimMapper.Map<ObjectId>(version);
            var versionsCursor = await VersionsCol
                .Find(e => e.Entity.Version == versionObjId)
                .ToCursorAsync();

            // there might be more then 1 document changed in 1 operation
            // e.g. updating parent (all children being modified with new Path property)
            while (await versionsCursor.MoveNextAsync())
            {
                foreach (var versionDo in versionsCursor.Current)
                {
                    var docDo = GimMapper.Map<CategoryDo>(versionDo);
                    var filter = Builders<CategoryDo>.Filter.Eq(x => x.Id, docDo.Id);
                    await Col.FindOneAndReplaceAsync(filter, docDo);
                }
            }
        }

        public async Task DeleteManyAsync()
        {
            await Col.DeleteManyAsync(Builders<CategoryDo>.Filter.Empty);
        }

        public async Task<Category> UpdateParentAsync(string id, string newParentId)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var versionObjId = ObjectId.GenerateNewId();
            var updateDefinition = Builders<CategoryDo>.Update.Set(x => x.Version, versionObjId);

            string newPath = null;
            var newParentObjId = ObjectId.Empty;

            if (!string.IsNullOrWhiteSpace(newParentId))
            {
                newParentObjId = GimMapper.Map<ObjectId>(newParentId);
                var f = Builders<CategoryDo>.Filter.Eq(x => x.Id, newParentObjId);
                var newParent = await Col.Find(f).FirstAsync();
                newPath = $"{newParent.Path}/{newParent.Id}";
            }


            var filter = Builders<CategoryDo>.Filter.Eq(x => x.Id, objId);
            var categoryDo = await Col.Find(filter).FirstAsync();
            var oldPath = categoryDo.Path;

            // get children that needs to update
            filter = Builders<CategoryDo>.Filter.Regex(x => x.Path, $"/^{categoryDo.Path}\\/{categoryDo.Id}/");
            var childrenDo = Col.Find(filter).ToCursor();

            // 'materialized path' pattern requires to update every child's path on every node move
            while (await childrenDo.MoveNextAsync())
            {
                var batch = childrenDo.Current;
                foreach (var child in batch)
                {
                    filter = Builders<CategoryDo>.Filter.Eq(x => x.Id, child.Id);
                    // if it is top-level node (in root), add new parent's path at the begin of old path
                    // else - replace old parent's part of path
                    var value = string.IsNullOrWhiteSpace(oldPath)
                        ? $"{newPath}{child.Path}"
                        : child.Path.Replace(oldPath, newPath);
                    var updateChildPath = updateDefinition.Set(x => x.Path, value);
                    await UpdateOneAsync(filter, updateChildPath);
                }
            }

            // finally, update path and ParentId on entity being moved
            filter = Builders<CategoryDo>.Filter.Eq(x => x.Id, objId);
            var update = updateDefinition
                .Set(x => x.ParentId, newParentObjId)
                .Set(x => x.Path, newPath);
            await UpdateOneAsync(filter, update);


            return GimMapper.Map<Category>(categoryDo);
        }

        public async Task<GetAllResult<Category>> GetVersions(int page, int pageSize)
        {
            var sort = Builders<CategoryVersionDo>.Sort.Descending(x => x.Entity.Version);
            var entities = await VersionsCol
                .Aggregate()
                .Sort(sort)
                .Lookup<CategoryDo, CategoryVersionFullDo>(
                    CollectionName,
                    new ExpressionFieldDefinition<CategoryVersionDo, ObjectId>(x => x.Entity.ParentId),
                    new ExpressionFieldDefinition<CategoryDo, ObjectId>(x => x.Id),
                    new ExpressionFieldDefinition<CategoryVersionFullDo, IEnumerable<CategoryDo>>(x =>
                        x.Entity.Parents))
                .Skip(page * pageSize)
                .Limit(pageSize)
                .As<CategoryVersionFullDo>()
                .ToListAsync();

            var result = new GetAllResult<Category>
            {
                Count = VersionsCol.CountDocuments(FilterDefinition<CategoryVersionDo>.Empty),
                Entities = GimMapper.Map<List<Category>>(entities)
            };

            return result;
        }

        public async Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items)
        {
            var categoryNames = items
                .SelectMany(x => new List<string>
                    {x.Category1Name, x.Category2Name, x.Category3Name, x.Category4Name, x.Category5Name})
                .Distinct()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            var filter = new CategoryFilter {Names = categoryNames};
            var categories = await GetChildrenFlattenAsync(filter);
            var categoriesDict = categories
                .SelectMany(x =>
                {
                    var names = new List<KeyValuePair<string, Category>>
                        {new KeyValuePair<string, Category>(x.Name, x)};
                    names.AddRange(x.Mappings.Select(m => new KeyValuePair<string, Category>(m.Name, x)));
                    return names;
                })
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.First().Value);

            items = items.Select(item =>
            {
                var category1 = TryGetCategory(categoriesDict, item.Category1Name);
                item.Category1Id = category1?.Id;
                item.Category1 = category1;

                var category2 = TryGetCategory(categoriesDict, item.Category2Name);
                item.Category2Id = category2?.Id;
                item.Category2 = category2;

                var category3 = TryGetCategory(categoriesDict, item.Category3Name);
                item.Category3Id = category3?.Id;
                item.Category3 = category3;

                var category4 = TryGetCategory(categoriesDict, item.Category4Name);
                item.Category4Id = category4?.Id;
                item.Category4 = category4;

                var category5 = TryGetCategory(categoriesDict, item.Category5Name);
                item.Category5Id = category5?.Id;
                item.Category5 = category5;

                return item;
            }).ToList();

            return items;
        }

        private IAggregateFluent<CategoryFullDo> AddLookups(IAggregateFluent<CategoryFullDo> af)
        {
            return af
                .Lookup<CategoryFullDo, CategoryFullDo>(
                    CollectionName,
                    nameof(CategoryDo.Id),
                    nameof(CategoryDo.ParentId),
                    nameof(CategoryFullDo.Children))
                .Lookup<CategoryFullDo, CategoryFullDo>(
                    ProductDao.CollectionName,
                    nameof(CategoryDo.Id),
                    nameof(ProductDo.CategoryId),
                    nameof(CategoryFullDo.Products))
                .As<CategoryFullDo>();
        }

        private static Category TryGetCategory(IReadOnlyDictionary<string, Category> categories, string name)
        {
            return !string.IsNullOrWhiteSpace(name) && categories.ContainsKey(name)
                ? categories[name]
                : null;
        }

        /// <summary>
        ///     Возвращает первый найденный идентификатор категории, начиная с последней (5)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetCategoryId(PriceListItemMatched item)
        {
            return string.IsNullOrWhiteSpace(item.Category5Id)
                ? string.IsNullOrWhiteSpace(item.Category4Id)
                    ? string.IsNullOrWhiteSpace(item.Category3Id)
                        ? string.IsNullOrWhiteSpace(item.Category2Id)
                            ? string.IsNullOrWhiteSpace(item.Category1Id)
                                ? null
                                : item.Category1Id
                            : item.Category2Id
                        : item.Category3Id
                    : item.Category4Id
                : item.Category5Id;
        }

        private async Task<int> GetMaxPosition()
        {
            var docDo = await Col
                .Aggregate()
                .Sort(Builders<CategoryDo>.Sort.Descending(x => x.Position))
                .Limit(1)
                .FirstOrDefaultAsync();
            return docDo?.Position ?? 0;
        }

        private async Task UpdateOneAsync(FilterDefinition<CategoryDo> filter, UpdateDefinition<CategoryDo> update)
        {
            await Col.UpdateOneAsync(filter, update);

            var docDo = Col.Find(filter).FirstOrDefault();
            var versionDo = new CategoryVersionDo
            {
                Entity = docDo
            };
            await VersionsCol.InsertOneAsync(versionDo);
        }
    }
}