using Gim.PriceParser.Bll.Common.Entities.CategoryProperties;
using Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues;

namespace Gim.PriceParser.Bll.Common.Entities.Products
{
    /// <summary>
    ///     Элемент характеристики товара
    /// </summary>
    public class ProductPropertyItem
    {
        /// <summary>
        ///     Свойство (характеристика)
        /// </summary>
        public CategoryProperty Property { get; set; }

        /// <summary>
        ///     Идентификатор свойства
        /// </summary>
        public string PropertyId { get; set; }

        /// <summary>
        ///     Значение характеристики
        /// </summary>
        public CategoryPropertyValue Value { get; set; }

        /// <summary>
        ///     Идентификатор значения характеристики
        /// </summary>
        public string ValueId { get; set; }
    }
}