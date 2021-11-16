using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Images;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class ImageDao : DaoBase<GimImage, ImageDo>, IImageDao
    {
        public const string CollectionName = "Images";

        public ImageDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao) : base(mapper, db,
            sequenceCounterDao, CollectionName)
        {
        }

        public async Task<GetAllResult<GimImage>> GetManyAsync(GimImageFilter filter, int page, int pageSize,
            bool withData = false)
        {
            var filterDo = GimMapper.Map<FilterDefinition<ImageDo>>(filter);

            var matched = Col
                .Aggregate()
                .Match(filterDo);

            var countResult = await matched
                .Count()
                .FirstOrDefaultAsync();

            if (pageSize > 0)
            {
                matched = matched
                    .Skip(page * pageSize)
                    .Limit(pageSize);
            }

            var docsDo = await matched.ToListAsync();

            var result = new GetAllResult<GimImage>
            {
                Count = countResult?.Count ?? 0,
                Entities = GimMapper.Map<List<GimImage>>(docsDo)
            };

            return result;
        }

        public async Task<GimImage> GetMainAsync(string productId)
        {
            var productObjId = GimMapper.Map<ObjectId>(productId);
            var filterDo = Builders<ImageDo>.Filter.Eq(x => x.ProductId, productObjId);

            var docDo = await Col.Find(filterDo).FirstOrDefaultAsync();
            if (docDo == null)
            {
                return null;
            }

            var doc = GimMapper.Map<GimImage>(docDo);
            return doc;
        }

        public async Task ReplaceManyAsync(List<GimImage> images)
        {
            var requests = images
                .Select(img =>
                {
                    var imgDo = GimMapper.Map<ImageDo>(img);
                    var filterDo = Builders<ImageDo>.Filter.Eq(x => x.Id, imgDo.Id);
                    return new ReplaceOneModel<ImageDo>(filterDo, imgDo);
                })
                .ToList();

            if (requests.Any())
            {
                await Col.BulkWriteAsync(requests);
            }
        }

        public async Task SetMainAsync(string id)
        {
            var objId = GimMapper.Map<ObjectId>(id);

            var buildersFilter = Builders<ImageDo>.Filter;
            var buildersUpdate = Builders<ImageDo>.Update;

            var newMainFilter = buildersFilter.Eq(x => x.Id, objId);

            // Получим изображение, которое сделаем главным
            var imageDo = await Col.Find(newMainFilter).FirstAsync();

            // Снимем признак прежнего главного изображения
            var filterDefinitions = new List<FilterDefinition<ImageDo>>
            {
                buildersFilter.Eq(x => x.ProductId, imageDo.ProductId),
                buildersFilter.Eq(x => x.IsMain, true)
            };

            var oldMainFilter = buildersFilter.And(filterDefinitions);
            var update = buildersUpdate.Set(x => x.IsMain, false);
            await Col.UpdateOneAsync(oldMainFilter, update);

            // Установим признак нового главного изображения
            update = buildersUpdate.Set(x => x.IsMain, true);
            await Col.UpdateOneAsync(newMainFilter, update);
        }

        public async Task SetPublishedAsync(string id, bool isPublished)
        {
            var objId = GimMapper.Map<ObjectId>(id);
            var filterDo = Builders<ImageDo>.Filter.Eq(x => x.Id, objId);
            var updateDo = Builders<ImageDo>.Update.Set(x => x.IsPublished, isPublished);

            await Col.UpdateOneAsync(filterDo, updateDo);
        }

        public async Task SetStatusManyAsync(GimImageFilter filter, GimImageDownloadStatus status)
        {
            var filterDo = GimMapper.Map<FilterDefinition<ImageDo>>(filter);
            var updateDo = Builders<ImageDo>.Update.Set(x => x.Status, status);

            await Col.UpdateManyAsync(filterDo, updateDo);
        }

        public async Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items)
        {
            var urls = items
                .SelectMany(item => item.Images.Select(img => img.Url))
                .Distinct()
                .ToList();

            var filterDo = Builders<ImageDo>.Filter.In(x => x.Url, urls);

            var imagesDo = await Col.Find(filterDo).ToListAsync();
            var images = GimMapper.Map<List<GimImage>>(imagesDo);
            var imagesDict = images
                .GroupBy(x => x.Url)
                .ToDictionary(x => x.Key, x => x.First());

            foreach (var item in items)
            {
                foreach (var itemImg in item.Images)
                {
                    itemImg.PriceListItemId = item.Id;

                    if (imagesDict.ContainsKey(itemImg.Url))
                    {
                        var gimImg = imagesDict[itemImg.Url];
                        itemImg.ImageId = gimImg.Id;
                        itemImg.Image = gimImg;
                    }
                }
            }

            return items;
        }

        public async Task<List<PriceListItemMatched>> AddAbsentItemsAsync(List<PriceListItemMatched> items)
        {
            var newItems = new List<GimImage>();

            var notSkipped = items.Where(x => !x.Skip);

            foreach (var item in notSkipped)
            {
                var itemImgs = item.Images
                    .Where(img => string.IsNullOrWhiteSpace(img.ImageId));
                foreach (var itemImg in itemImgs)
                {
                    var newGimImg = new GimImage
                    {
                        Id = GenerateNewObjectId(),
                        ProductId = item.ProductId,
                        Url = itemImg.Url,
                        Status = GimImageDownloadStatus.NotDownloaded
                    };

                    itemImg.Image = newGimImg;
                    itemImg.ImageId = newGimImg.Id;

                    newItems.Add(newGimImg);
                }
            }

            if (newItems.Any())
            {
                await AddManyAsync(newItems);
            }

            return items;
        }

        public async Task MergeProducts(List<string> productIds)
        {
            var productObjIds = GimMapper.Map<List<ObjectId>>(productIds);

            var filterDo = Builders<ImageDo>.Filter.In(x => x.ProductId, productObjIds.Skip(1));
            var updateDo = Builders<ImageDo>.Update.Set(x => x.ProductId, productObjIds.First());

            await Col.UpdateManyAsync(filterDo, updateDo);
        }

        public async Task DeleteManyAsync()
        {
            await Col.DeleteManyAsync(Builders<ImageDo>.Filter.Empty);
        }
    }
}