using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.CategoryProperties;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryProperty;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class CategoryPropertyDao : DaoBase<CategoryProperty, CategoryPropertyDo>, ICategoryPropertyDao
    {
        public const string CollectionName = "CategoryProperties";

        public CategoryPropertyDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao) : base(
            mapper, db, sequenceCounterDao, CollectionName)
        {
        }

        public async Task<GetAllResult<CategoryProperty>> GetManyAsync(CategoryPropertyFilter filter)
        {
            var filterDo = GimMapper.Map<FilterDefinition<CategoryPropertyDo>>(filter);

            var entitiesDo = await Col
                .Aggregate()
                .Match(filterDo)
                .ToListAsync();

            var result = new GetAllResult<CategoryProperty>
            {
                Count = Col.CountDocuments(filterDo),
                Entities = GimMapper.Map<List<CategoryProperty>>(entitiesDo)
            };

            return result;
        }

        public async Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items)
        {
            var keys = items.SelectMany(item => item.Properties.Select(property => property.PropertyKey)).Distinct();

            var filterDo = Builders<CategoryPropertyDo>.Filter.In(x => x.Key, keys);
            var propertiesDo = await Col.Find(filterDo).ToListAsync();

            items.ForEach(item =>
            {
                item.Properties.ForEach(property =>
                {
                    property.PriceListItemId = item.Id;
                    property.PropertyId = propertiesDo.FirstOrDefault(x => x.Key == property.PropertyKey)?.Id
                        .ToString();
                });
            });
            return items;
        }

        public async Task<List<PriceListItemMatched>> AddAbsentItemsAsync(List<PriceListItemMatched> items)
        {
            var newProperties = new List<CategoryProperty>();
            foreach (var item in items.Where(x => !x.Skip))
            {
                foreach (var property in item.Properties.Where(x =>
                    string.IsNullOrWhiteSpace(x.PropertyId) && x.Action == PriceListItemAction.CreateNew))
                {
                    var prop = new CategoryProperty
                    {
                        Id = GenerateNewObjectId(),
                        CategoryId = CategoryDao.GetCategoryId(item),
                        Key = property.PropertyKey,
                        Name = property.PropertyKey
                    };
                    newProperties.Add(prop);
                }
            }

            newProperties = newProperties.Distinct(new CategoryPropertyComparerByCategoryAndKey()).ToList();

            if (newProperties.Any())
            {
                newProperties = await AddManyAsync(newProperties);
            }

            items.ForEach(item =>
            {
                item.Properties.ForEach(property =>
                {
                    if (!string.IsNullOrWhiteSpace(property.PropertyId) || property.Action != PriceListItemAction.CreateNew)
                    {
                        return;
                    }

                    var newProperty = newProperties.First(x => x.Key == property.PropertyKey);
                    property.PropertyId = newProperty.Id;
                    property.Property = newProperty;
                });
            });

            return items;
        }

        public async Task DeleteManyAsync()
        {
            await Col.DeleteManyAsync(Builders<CategoryPropertyDo>.Filter.Empty);
        }
    }

    internal class CategoryPropertyComparerByCategoryAndKey : IEqualityComparer<CategoryProperty>
    {
        public bool Equals(CategoryProperty x, CategoryProperty y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Key == y.Key && x.CategoryId == y.CategoryId;
        }

        public int GetHashCode(CategoryProperty obj)
        {
            return obj.Key.GetHashCode() + obj.CategoryId.GetHashCode();
        }
    }
}