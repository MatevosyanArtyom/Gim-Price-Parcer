using Gim.PriceParser.Bll.Common.Entities.CategoryProperties;

namespace Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues
{
    /// <summary>
    ///     Занчение характеристики товара
    /// </summary>
    public class CategoryPropertyValue
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Числовой идентификатор
        /// </summary>
        public long SeqId { get; set; }

        /// <summary>
        ///     Характеристика товара
        /// </summary>
        public CategoryProperty Property { get; set; }

        /// <summary>
        ///     Идентификатор характеристики
        /// </summary>
        public string PropertyId { get; set; }

        /// <summary>
        ///     Наименование
        /// </summary>
        public string Name { get; set; }
    }
}