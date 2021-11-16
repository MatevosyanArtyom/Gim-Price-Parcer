using Gim.PriceParser.Dal.Impl.Mongo.DbSettings;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace Gim.PriceParser.Bll.Scheduler
{
    public static class SchedulerModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire((sp, cfg) =>
            {
                var storageOptions = new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy()
                    }
                };

                var mongoDbSettings = sp.GetService<IMongoDbSettings>();
                cfg.UseMongoStorage(mongoDbSettings.GetMongoClientSettings(), "HangfireStorage", storageOptions);
            });

            services.AddHangfireServer(opt =>
            {
                opt.WorkerCount = 1;
                opt.Queues = new[] {HangfireQueues.Parser};
            });

            services.AddHangfireServer(opt =>
            {
                opt.WorkerCount = 1;
                opt.Queues = new[] {HangfireQueues.ImageDownloader};
            });

            services.AddTransient<ISchedulerClient, SchedulerClient>();
        }
    }
}