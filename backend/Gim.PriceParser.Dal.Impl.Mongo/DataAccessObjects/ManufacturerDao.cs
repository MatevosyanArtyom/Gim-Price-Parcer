using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects
{
    internal class ManufacturerDao : DaoBase<Manufacturer, ManufacturerDo>, IManufacturerDao
    {
        public const string CollectionName = "Manufacturers";

        public ManufacturerDao(IMapper mapper, IGimDbContext db, ISequenceCounterDao sequenceCounterDao) : base(mapper,
            db, sequenceCounterDao, CollectionName)
        {
        }
    }
}