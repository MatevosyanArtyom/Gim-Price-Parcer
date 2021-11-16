using Gim.PriceParser.Dal.Common;
using Gim.PriceParser.Dal.Common.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.Abstractions;
using Gim.PriceParser.Dal.Impl.Mongo.DataAccessObjects;
using Gim.PriceParser.Dal.Impl.Mongo.DbContext;
using Microsoft.Extensions.DependencyInjection;

namespace Gim.PriceParser.Dal.Impl.Mongo
{
    public static class DalModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGimDbContext, GimDbContext>();
            services.AddSingleton<ISequenceCounterDao, SequenceCounterDao>();
            services.AddSingleton<IDbConfigurer, DbConfigurer>();
            services.AddSingleton<IDbSeeder, DbSeeder>();

            services.AddTransient(typeof(IArchivableDao<>), typeof(ArchivableDao<>));
            services.AddTransient<ICategoryDao, CategoryDao>();
            services.AddTransient<IImageDao, ImageDao>();
            services.AddTransient<IManufacturerDao, ManufacturerDao>();
            services.AddTransient<IPriceListDao, PriceListDao>();
            services.AddTransient<IPriceListItemDao, PriceListItemDao>();
            services.AddTransient<IPriceListItemImageDao, PriceListItemImageDao>();
            services.AddTransient<IPriceListItemPropertyDao, PriceListItemPropertyDao>();
            services.AddTransient<IProcessingRuleDao, ProcessingRuleDao>();
            services.AddTransient<IProductDao, ProductDao>();
            services.AddTransient<ICategoryPropertyDao, CategoryPropertyDao>();
            services.AddTransient<ICategoryPropertyValueDao, CategoryPropertyValueDao>();
            services.AddTransient<ISchedulerTaskDao, SchedulerTaskDao>();
            services.AddTransient<ISupplierDao, SupplierDao>();
            services.AddTransient<ISupplierProductDao, SupplierProductDao>();
            services.AddTransient<IUserDao, UserDao>();
            services.AddTransient<IUserRoleDao, UserRoleDao>();
        }
    }
}