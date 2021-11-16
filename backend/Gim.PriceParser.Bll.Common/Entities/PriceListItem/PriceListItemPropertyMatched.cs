using Gim.PriceParser.Bll.Common.Entities.CategoryProperties;
using Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues;

namespace Gim.PriceParser.Bll.Common.Entities.PriceListItem
{
    /// <summary>
    ///     Сопоставленный элемент характеристики товара
    /// </summary>
    public class PriceListItemPropertyMatched
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
        ///     Идентификатор строки прайс-листа
        /// </summary>
        public string PriceListItemId { get; set; }

        /// <summary>
        ///     Ключ для поиска характеристики
        /// </summary>
        public string PropertyKey { get; set; }

        /// <summary>
        ///     Сопоставленный элемент характеристики
        /// </summary>
        public CategoryProperty Property { get; set; }

        /// <summary>
        ///     Идентификатор сопоставленной характеристики
        /// </summary>
        public string PropertyId { get; set; }

        /// <summary>
        ///     Значение характеристики (из прайс-листа)
        /// </summary>
        public string PropertyValue { get; set; }

        /// <summary>
        ///     Значение характеристики у элемента номенклатуры
        /// </summary>
        public CategoryPropertyValue ProductValue { get; set; }

        /// <summary>
        ///     Идентификатор значения характеристики у элемента номенклатуры
        /// </summary>
        public string ProductValueId { get; set; }

        /// <summary>
        ///     Сопоставленный элемент значения характеристики
        /// </summary>
        public CategoryPropertyValue Value { get; set; }

        /// <summary>
        ///     Идентификатор сопоставленного значения характеристики
        /// </summary>
        public string ValueId { get; set; }

        /// <summary>
        ///     Статус свойства элемента прайс-листа
        /// </summary>
        public PriceListItemStatus Status { get; set; }

        /// <summary>
        ///     Действие, которое будет выполнено при загрузке свойства в БД
        /// </summary>
        public PriceListItemAction Action { get; set; }
    }
}