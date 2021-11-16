using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;

namespace Gim.PriceParser.Bll.Search
{
    public static class SearchModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IElasticClient, ElasticClient>(sp =>
            {
                var settings = sp.GetService<IOptions<ElasticSearchSettings>>();

                var options = new ConnectionSettings(new Uri(settings.Value.ConnectionString));
                options.DefaultIndex(settings.Value.DefaultIndex);

                return new ElasticClient(options);
            });

            services.AddSingleton<IElasticConfigurer, ElasticConfigurer>();
            services.AddTransient<ISearchClient, SearchClient>();
        }
    }
}