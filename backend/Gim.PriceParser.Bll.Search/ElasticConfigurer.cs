using Gim.PriceParser.Bll.Search.Models;
using Microsoft.Extensions.Options;
using Nest;

namespace Gim.PriceParser.Bll.Search
{
    internal class ElasticConfigurer : IElasticConfigurer
    {
        private readonly IElasticClient _client;
        private readonly ElasticSearchSettings _settings;

        public ElasticConfigurer(IOptions<ElasticSearchSettings> options, IElasticClient client)
        {
            _settings = options.Value;
            _client = client;
        }

        public void Configure()
        {
            var response = _client.Indices.Exists(_settings.DefaultIndex);
            if (!response.Exists)
            {
                _client.Indices.Create(_settings.DefaultIndex, c =>
                {
                    c = c.Settings(s =>
                        s.Analysis(a =>
                            a.Normalizers(n => n.Custom("lowercase_norm", cnd => cnd.Filters("lowercase")))));
                    c = c.Map<ProductEs>(m => m.AutoMap());
                    return c;
                });
            }
        }
    }
}