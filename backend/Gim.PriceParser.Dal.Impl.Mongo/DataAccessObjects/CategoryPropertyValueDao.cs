using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryProperty;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryPropertyValue;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class CategoryPropertyValueDao : DaoBase<CategoryPropertyValue, CategoryPropertyValueDo>,
        ICategoryPropertyValueDao
    {
        public const string CollectionName = "CategoryPropertyValues";

        public CategoryPropertyValueDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao) :
            base(mapper, db, sequenceCounterDao, CollectionName)
        {
        }

        public async Task<List<CategoryPropertyValue>> GetManyAsync(CategoryPropertyValueFilter filter)
        {
            var filterDo = GimMapper.Map<FilterDefinition<CategoryPropertyValueDo>>(filter);

            var entitiesDo = await Col
                .Aggregate()
                .Match(filterDo)
                .Lookup<CategoryPropertyDo, CategoryPropertyValueDo>(
                    CategoryPropertyDao.CollectionName,
                    nameof(CategoryPropertyValueDo.PropertyId),
                    nameof(CategoryPropertyDo.Id),
                    nameof(CategoryPropertyValueFullDo.Properties))
                .As<CategoryPropertyValueFullDo>()
                .ToListAsync();

            var entities = GimMapper.Map<List<CategoryPropertyValue>>(entitiesDo);

            return entities;
        }

        public async Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items)
        {
            var names = items.SelectMany(item => item.Properties.Select(property => property.PropertyValue)).Distinct();
            var ids = items.SelectMany(item => item.Properties
                    .Where(property => !string.IsNullOrWhiteSpace(property.PropertyId))
                    .Select(property => property.PropertyId))
                .Distinct();
            var idsDo = GimMapper.Map<List<ObjectId>>(ids);

            var filterDo = Builders<CategoryPropertyValueDo>.Filter.And(
                Builders<CategoryPropertyValueDo>.Filter.In(x => x.Name, names),
                Builders<CategoryPropertyValueDo>.Filter.In(x => x.PropertyId, idsDo));

            var valuesDo = await Col.Find(filterDo).ToListAsync();

            items.ForEach(item =>
            {
                item.Properties.ForEach(property =>
                {
                    var productProperty =
                        item.Product?.Properties.FirstOrDefault(x => x.PropertyId == property.PropertyId);
                    if (productProperty != null)
                    {
                        property.ProductValueId = productProperty.Id;
                        property.ProductValue = productProperty;
                    }

                    property.ValueId = valuesDo.FirstOrDefault(x => x.Name == property.PropertyValue)?.Id
                        .ToString();
                });
            });
            return items;
        }

        public async Task<List<PriceListItemMatched>> AddAbsentItemsAsync(List<PriceListItemMatched> items)
        {
            var allValues = items
                .Where(item => !item.Skip)
                .SelectMany(item => item.Properties
                    .Where(property => !string.IsNullOrWhiteSpace(property.PropertyId)))
                .Distinct(new PriceListItemPropertyMatchedComparerByValue())
                .ToList();

            var newValues = allValues
                .Where(v => string.IsNullOrWhiteSpace(v.ValueId) && !string.IsNullOrWhiteSpace(v.PropertyValue))
                .ToList();

            newValues.ForEach(v => { v.ValueId = GenerateNewObjectId(); });

            if (newValues.Any())
            {
                var valuesDo = GimMapper.Map<List<CategoryPropertyValueDo>>(newValues);
                await Col.InsertManyAsync(valuesDo);
            }

            items.ForEach(item =>
            {
                item.Properties.ForEach(property =>
                {
                    var value = allValues.FirstOrDefault(v =>
                        v.PropertyId == property.PropertyId && v.PropertyValue == property.PropertyValue);
                    if (value == null)
                    {
                        return;
                    }

                    property.ValueId = value.ValueId;
                    property.Value = new CategoryPropertyValue
                    {
                        Id = value.ValueId,
                        Name = value.PropertyValue,
                        PropertyId = value.PropertyId
                    };
                });
            });

            return items;
        }

        public async Task DeleteManyAsync()
        {
            await DeleteMany(Builders<CategoryPropertyValueDo>.Filter.Empty);
        }

        public async Task DeleteManyAsync(string propertyId)
        {
            var propertyObjId = GimMapper.Map<ObjectId>(propertyId);
            var filterDo = Builders<CategoryPropertyValueDo>.Filter.Eq(x => x.PropertyId, propertyObjId);
            await DeleteMany(filterDo);
        }

        private async Task DeleteMany(FilterDefinition<CategoryPropertyValueDo> filterDo)
        {
            await Col.DeleteManyAsync(filterDo);
        }
    }

    internal class PriceListItemPropertyMatchedComparerByValue : IEqualityComparer<PriceListItemPropertyMatched>
    {
        public bool Equals(PriceListItemPropertyMatched x, PriceListItemPropertyMatched y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.PropertyId == y.PropertyId && x.PropertyValue == y.PropertyValue;
        }

        public int GetHashCode(PriceListItemPropertyMatched obj)
        {
            return obj.PropertyId?.GetHashCode() ?? 0 + obj.PropertyValue.GetHashCode();
        }
    }
}