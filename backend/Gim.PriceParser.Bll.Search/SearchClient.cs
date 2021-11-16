using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Gim.PriceParser.Bll.Search.Models;
using Gim.PriceParser.Bll.Search.Utils;
using Nest;

namespace Gim.PriceParser.Bll.Search
{
    public class SearchClient : ISearchClient
    {
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public SearchClient(IElasticClient elasticClient, IMapper mapper)
        {
            _elasticClient = elasticClient;
            _mapper = mapper;
        }

        public async Task AddManyAsync(List<Product> products)
        {
            var docsEs = _mapper.Map<List<ProductEs>>(products);
            await _elasticClient.IndexManyAsync(docsEs);
        }

        public async Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items)
        {
            Expression<Func<ProductEs, object>> exp = p => p.Name;
            var field = new Field(exp);

            var filtered = items
                .Where(x => string.IsNullOrWhiteSpace(x.ProductId) && !string.IsNullOrWhiteSpace(x.ProductName))
                .GroupBy(x => x.ProductName) // DistinctBy ProductName
                .Select(g => g.First())
                .ToList();

            var msd = new MultiSearchDescriptor();

            foreach (var item in filtered)
            {
                Func<QueryContainerDescriptor<ProductEs>, QueryContainer> PropToQuerySelector(
                    PriceListItemPropertyMatched prop)
                {
                    return qcd => qcd.Term(s => s.Field(field).Value(prop.PropertyValue));
                }

                var propQueries = item.Properties
                    .Where(prop => !string.IsNullOrEmpty(prop.PropertyValue))
                    .Select(PropToQuerySelector);
                var queries = new List<Func<QueryContainerDescriptor<ProductEs>, QueryContainer>>
                {
                    qcd => qcd.Match(s => s.Field(field).Query(item.ProductName)) // name match query
                };
                queries.AddRange(propQueries); // properties term queries

                // ReSharper disable once ConvertToLocalFunction
                Func<QueryContainerDescriptor<ProductEs>, QueryContainer> categoryQuery = qcd =>
                    qcd.Term(s => s.Field(x => x.CategoryId).Value(item.CategoryId));

                msd.Search<ProductEs>(item.ProductName,
                    ss => ss.Query(q => q.Bool(bs => bs.Should(queries).Must(categoryQuery))).From(0).Size(5));
            }

            var multiSearchResponse = await _elasticClient.MultiSearchAsync(msd);

            var i = 0;
            foreach (var response in multiSearchResponse.AllResponses)
            {
                var searchResponse = (SearchResponse<ProductEs>) response;
                filtered[i].ProductSynonyms = searchResponse.Hits
                    .Select(doc => new ProductSynonym {ProductId = doc.Source.InnerId, Score = doc.Score ?? 0})
                    .ToList();

                i++;
            }

            return items;
        }

        public async Task SetCategoryManyAsync(string fromId, string toId)
        {
            Expression<Func<ProductEs, string>> exp = p => p.CategoryId;
            var field = new Field(exp);

            QueryContainer Qs(QueryContainerDescriptor<ProductEs> qcd)
            {
                return qcd.Term(s => s.Field(field).Value(fromId));
            }

            var result = await _elasticClient.UpdateByQueryAsync<ProductEs>(uqd =>
                uqd.Query(Qs).Script($"ctx._source.{nameof(ProductEs.CategoryId).ToCamelCase()}='{toId}'"));
        }


        public async Task DeleteManyAsync(ProductFilter filter = null)
        {
            Expression<Func<ProductEs, string>> exp = p => p.InnerId;
            var field = new Field(exp);

            QueryContainer Qs(QueryContainerDescriptor<ProductEs> qcd)
            {
                if (filter == null)
                {
                    return qcd.MatchAll();
                }

                if (filter.Ids.Any())
                {
                    return qcd.Terms(s => s.Field(field).Terms(filter.Ids));
                }

                return qcd.MatchAll();
            }

            await _elasticClient.DeleteByQueryAsync<ProductEs>(dqr => dqr.Query(Qs));
        }
    }
}