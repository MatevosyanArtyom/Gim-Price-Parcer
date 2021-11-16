using System.Collections.Generic;
using Gim.PriceParser.WebApi.Models.ProductPropertyValue;

namespace Gim.PriceParser.WebApi.Models.Product
{
    public class ProductPropertiesModel
    {
        /// <summary>
        ///     Список значений характеристик у номенклатуры
        /// </summary>
        public List<ProductPropertyValueLookup> Values { get; set; }

        /// <summary>
        ///     Все значения характеристик.
        ///     Значения присутствуют только для тех характеристик, которые есть у номенклатуры
        /// </summary>
        public List<ProductPropertyValueLookup> AllValues { get; set; }
    }
}