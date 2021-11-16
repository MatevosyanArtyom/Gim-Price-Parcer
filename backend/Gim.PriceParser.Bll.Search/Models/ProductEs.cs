using System.Collections.Generic;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Nest;

namespace Gim.PriceParser.Bll.Search.Models
{
    [ElasticsearchType(RelationName = nameof(Product))]
    public class ProductEs
    {
        public string Id { get; set; }

        /// <summary>
        ///     Идентификатор в БД-хранилище (mongo)
        /// </summary>
        [Keyword]
        public string InnerId { get; set; }

        /// <summary>
        ///     Идентификатор категории
        /// </summary>
        [Keyword]
        public string CategoryId { get; set; }

        /// <summary>
        ///     Наименование товара
        /// </summary>
        [Text(Analyzer = "russian")]
        public string Name { get; set; }

        /// <summary>
        ///     Список значений свойств
        /// </summary>
        [Keyword(Normalizer = "lowercase_norm")]
        public List<string> Properties { get; set; } = new List<string>();
    }
}