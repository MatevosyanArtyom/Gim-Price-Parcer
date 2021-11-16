using System.Collections.Generic;
using Gim.PriceParser.Bll.Common.Entities.Categories;
using Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues;
using Gim.PriceParser.Bll.Common.Entities.Suppliers;

namespace Gim.PriceParser.Bll.Common.Entities.Products
{
    /// <summary>
    ///     Товар
    /// </summary>
    public class Product
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
        ///     Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Категория
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        ///     Идентификатор категории
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        ///     Поставщик
        /// </summary>
        public Supplier Supplier { get; set; }

        /// <summary>
        ///     Идентификатор поставщика
        /// </summary>
        public string SupplierId { get; set; }

        /// <summary>
        ///     Цена у поставщиков, от
        /// </summary>
        public decimal? PriceFrom { get; set; }

        /// <summary>
        ///     Количество поставщиков
        /// </summary>
        public long SupplierCount { get; set; }

        /// <summary>
        ///     Общее количество изображений
        /// </summary>
        public long ImageTotalCount { get; set; }

        /// <summary>
        ///     Количество опубликованных изображений
        /// </summary>
        public long ImagePublishedCount { get; set; }

        /// <summary>
        ///     Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Список свойств (характеристик)
        /// </summary>
        public List<CategoryPropertyValue> Properties { get; set; } = new List<CategoryPropertyValue>();

        /// <summary>
        ///     Статус
        /// </summary>
        public EntityStatus Status { get; set; }

        /// <summary>
        ///     Версия
        /// </summary>
        public string Version { get; set; }
    }
}