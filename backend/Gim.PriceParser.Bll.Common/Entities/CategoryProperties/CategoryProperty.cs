using Gim.PriceParser.Bll.Common.Entities.Categories;

namespace Gim.PriceParser.Bll.Common.Entities.CategoryProperties
{
    /// <summary>
    ///     Характеристика товара
    /// </summary>
    public class CategoryProperty
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
        ///     Категория товаров
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        ///     Идентификатор категории
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        ///     Ключ. Используется в скриптах обработки прайсов
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Наименование
        /// </summary>
        public string Name { get; set; }
    }
}