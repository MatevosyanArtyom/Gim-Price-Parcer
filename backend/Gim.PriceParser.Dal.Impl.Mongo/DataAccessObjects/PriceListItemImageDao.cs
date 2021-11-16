using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceListItem;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Driver;


namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class PriceListItemImageDao : DaoBase<PriceListItemImage, PriceListItemImageDo>, IPriceListItemImageDao
    {
        public const string CollectionName = "PriceListItemImages";

        public PriceListItemImageDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao) : base(
            mapper, db, sequenceCounterDao, CollectionName)
        {
        }

        public async Task<List<PriceListItemImage>> GetManyAsync(PriceListItemImageFilter filter)
        {
            var filterDo = GimMapper.Map<FilterDefinition<PriceListItemImageDo>>(filter);
            var query = Col
                .Aggregate()
                .Match(filterDo)
                .Lookup<ImageDo, PriceListItemImageDo>(
                    ImageDao.CollectionName,
                    nameof(PriceListItemImageDo.ImageId),
                    nameof(ImageDo.Id),
                    nameof(PriceListItemImageFullDo.GimImages));

            var entitiesDo = await query
                .As<PriceListItemImageFullDo>()
                .ToListAsync();

            var entities = GimMapper.Map<List<PriceListItemImage>>(entitiesDo);
            return entities;
        }

        public async Task DeleteManyAsync(PriceListItemImageFilter filter)
        {
            var filterDo = GimMapper.Map<FilterDefinition<PriceListItemImageDo>>(filter);

            await Col.DeleteManyAsync(filterDo);
        }
    }
}