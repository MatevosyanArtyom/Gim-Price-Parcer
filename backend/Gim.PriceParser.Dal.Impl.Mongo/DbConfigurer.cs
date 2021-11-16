using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Dal.Common;
using Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Category;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryProperty;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.CategoryPropertyValue;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceListItem;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Product;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SupplierProduct;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo
{
    internal class DbConfigurer : IDbConfigurer
    {
        private readonly IGimDbContext _db;

        public DbConfigurer(IGimDbContext db)
        {
            _db = db;
        }

        public async Task ConfigureIndexes()
        {
            await ConfigureCategoryIndexes();
            await ConfigureCategoryPropertyIndexes();
            await ConfigureCategoryPropertyValueIndexes();
            await ConfigureImageIndexes();
            await ConfigurePriceListItemIndexes();
            await ConfigurePriceListItemImageIndexes();
            await ConfigurePriceListItemPropertyIndexes();
            await ConfigureProductIndexes();
            await ConfigureSupplierIndexes(); 
            await ConfigureSupplierProductIndexes(); 
        }

        private async Task ConfigureCategoryIndexes()
        {
            var col = _db.GetCollection<CategoryDo>(CategoryDao.CollectionName);
            var indexes = new List<CreateIndexModel<CategoryDo>>
            {
                new CreateIndexModel<CategoryDo>(Builders<CategoryDo>.IndexKeys.Ascending(x => x.ParentId)),
                new CreateIndexModel<CategoryDo>(Builders<CategoryDo>.IndexKeys.Ascending(x => x.Name))
            };
            await col.Indexes.CreateManyAsync(indexes);
        }

        private async Task ConfigureCategoryPropertyIndexes()
        {
            var col = _db.GetCollection<CategoryPropertyDo>(CategoryPropertyDao.CollectionName);
            var indexes = new List<CreateIndexModel<CategoryPropertyDo>>
            {
                new CreateIndexModel<CategoryPropertyDo>(
                    Builders<CategoryPropertyDo>.IndexKeys.Ascending(x => x.CategoryId)),
                new CreateIndexModel<CategoryPropertyDo>(Builders<CategoryPropertyDo>.IndexKeys.Ascending(x => x.Key))
            };
            await col.Indexes.CreateManyAsync(indexes);
        }

        private async Task ConfigureCategoryPropertyValueIndexes()
        {
            var col = _db.GetCollection<CategoryPropertyValueDo>(CategoryPropertyValueDao.CollectionName);
            var indexes = new List<CreateIndexModel<CategoryPropertyValueDo>>
            {
                new CreateIndexModel<CategoryPropertyValueDo>(
                    Builders<CategoryPropertyValueDo>.IndexKeys.Ascending(x => x.PropertyId)),
                new CreateIndexModel<CategoryPropertyValueDo>(
                    Builders<CategoryPropertyValueDo>.IndexKeys.Ascending(x => x.Name))
            };
            await col.Indexes.CreateManyAsync(indexes);
        }

        private async Task ConfigureImageIndexes()
        {
            var col = _db.GetCollection<ImageDo>(ImageDao.CollectionName);
            var indexes = new List<CreateIndexModel<ImageDo>>
            {
                new CreateIndexModel<ImageDo>(Builders<ImageDo>.IndexKeys.Ascending(x => x.IsMain)),
                new CreateIndexModel<ImageDo>(Builders<ImageDo>.IndexKeys.Ascending(x => x.IsPublished)),
                new CreateIndexModel<ImageDo>(Builders<ImageDo>.IndexKeys.Ascending(x => x.ProductId)),
                new CreateIndexModel<ImageDo>(Builders<ImageDo>.IndexKeys.Ascending(x => x.Url)),
                new CreateIndexModel<ImageDo>(Builders<ImageDo>.IndexKeys.Ascending(x => x.Status))
            };
            await col.Indexes.CreateManyAsync(indexes);
        }

        private async Task ConfigurePriceListItemIndexes()
        {
            var col = _db.GetCollection<PriceListItemDo>(PriceListItemDao.CollectionName);
            var indexes = new List<CreateIndexModel<PriceListItemDo>>
            {
                new CreateIndexModel<PriceListItemDo>(Builders<PriceListItemDo>.IndexKeys.Ascending(x => x.PriceListId)),
                new CreateIndexModel<PriceListItemDo>(Builders<PriceListItemDo>.IndexKeys.Ascending(x => x.Category1Name)),
                new CreateIndexModel<PriceListItemDo>(Builders<PriceListItemDo>.IndexKeys.Ascending(x => x.Category2Name)),
                new CreateIndexModel<PriceListItemDo>(Builders<PriceListItemDo>.IndexKeys.Ascending(x => x.Category3Name)),
                new CreateIndexModel<PriceListItemDo>(Builders<PriceListItemDo>.IndexKeys.Ascending(x => x.Category4Name)),
                new CreateIndexModel<PriceListItemDo>(Builders<PriceListItemDo>.IndexKeys.Ascending(x => x.Category5Name)),
                new CreateIndexModel<PriceListItemDo>(Builders<PriceListItemDo>.IndexKeys.Ascending(x => x.ProductName)),
                new CreateIndexModel<PriceListItemDo>(Builders<PriceListItemDo>.IndexKeys.Ascending(x => x.Price1)),
                new CreateIndexModel<PriceListItemDo>(Builders<PriceListItemDo>.IndexKeys.Ascending(x => x.Price2)),
                new CreateIndexModel<PriceListItemDo>(Builders<PriceListItemDo>.IndexKeys.Ascending(x => x.Price3)),
                new CreateIndexModel<PriceListItemDo>(Builders<PriceListItemDo>.IndexKeys.Ascending(x => x.Status)),
            };
            await col.Indexes.CreateManyAsync(indexes);
        }

        private async Task ConfigurePriceListItemImageIndexes()
        {
            var col = _db.GetCollection<PriceListItemImageDo>(PriceListItemImageDao.CollectionName);
            var indexes = new List<CreateIndexModel<PriceListItemImageDo>>
            {
                new CreateIndexModel<PriceListItemImageDo>(
                    Builders<PriceListItemImageDo>.IndexKeys.Ascending(x => x.ImageId)),
                new CreateIndexModel<PriceListItemImageDo>(
                    Builders<PriceListItemImageDo>.IndexKeys.Ascending(x => x.PriceListItemId))
            };
            await col.Indexes.CreateManyAsync(indexes);
        }

        private async Task ConfigurePriceListItemPropertyIndexes()
        {
            var col = _db.GetCollection<PriceListItemPropertyDo>(PriceListItemPropertyDao.CollectionName);
            var indexes = new List<CreateIndexModel<PriceListItemPropertyDo>>
            {
                new CreateIndexModel<PriceListItemPropertyDo>(
                    Builders<PriceListItemPropertyDo>.IndexKeys.Ascending(x => x.PriceListItemId))
            };
            await col.Indexes.CreateManyAsync(indexes);
        }

        private async Task ConfigureProductIndexes()
        {
            var col = _db.GetCollection<ProductDo>(ProductDao.CollectionName);
            var indexes = new List<CreateIndexModel<ProductDo>>
            {
                new CreateIndexModel<ProductDo>(Builders<ProductDo>.IndexKeys.Ascending(x => x.SeqId)),
                new CreateIndexModel<ProductDo>(Builders<ProductDo>.IndexKeys.Ascending(x => x.CategoryId)),
                new CreateIndexModel<ProductDo>(Builders<ProductDo>.IndexKeys.Ascending(x => x.Name)),
                new CreateIndexModel<ProductDo>(Builders<ProductDo>.IndexKeys.Ascending(x => x.Status)),
                new CreateIndexModel<ProductDo>(Builders<ProductDo>.IndexKeys.Text(x => x.Name), new CreateIndexOptions{DefaultLanguage = "russian"})
            };
            await col.Indexes.CreateManyAsync(indexes);
        }

        private async Task ConfigureSupplierIndexes()
        {
            var col = _db.GetCollection<SupplierDo>(SupplierDao.CollectionName);
            var indexes = new List<CreateIndexModel<SupplierDo>>
            {
                new CreateIndexModel<SupplierDo>(Builders<SupplierDo>.IndexKeys.Ascending(x => x.SeqId)),
                new CreateIndexModel<SupplierDo>(Builders<SupplierDo>.IndexKeys.Ascending(x => x.CreatedDate)),
                new CreateIndexModel<SupplierDo>(Builders<SupplierDo>.IndexKeys.Ascending(x => x.Name)),
                new CreateIndexModel<SupplierDo>(Builders<SupplierDo>.IndexKeys.Ascending(x => x.Inn)),
                new CreateIndexModel<SupplierDo>(Builders<SupplierDo>.IndexKeys.Ascending(x => x.City.Value)),
                new CreateIndexModel<SupplierDo>(Builders<SupplierDo>.IndexKeys.Ascending(x => x.Status)),
                new CreateIndexModel<SupplierDo>(Builders<SupplierDo>.IndexKeys.Ascending(x => x.IsArchived))
            };
            await col.Indexes.CreateManyAsync(indexes);
        }

        private async Task ConfigureSupplierProductIndexes()
        {
            var col = _db.GetCollection<SupplierProductDo>(SupplierProductDao.CollectionName);
            var indexes = new List<CreateIndexModel<SupplierProductDo>>
            {
                new CreateIndexModel<SupplierProductDo>(Builders<SupplierProductDo>.IndexKeys.Ascending(x => x.SeqId)),
                new CreateIndexModel<SupplierProductDo>(Builders<SupplierProductDo>.IndexKeys.Ascending(x => x.ProductId)),
                new CreateIndexModel<SupplierProductDo>(Builders<SupplierProductDo>.IndexKeys.Ascending(x => x.SupplierId))
            };
            await col.Indexes.CreateManyAsync(indexes);
        }
    }
}