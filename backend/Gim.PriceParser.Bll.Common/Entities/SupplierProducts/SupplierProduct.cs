using System.Collections.Generic;
using Gim.PriceParser.Bll.Common.Entities.CategoryPropertyValues;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Gim.PriceParser.Bll.Common.Entities.Suppliers;

namespace Gim.PriceParser.Bll.Common.Entities.SupplierProducts
{
    /// <summary>
    ///     Содержит информацию о цене и наличии товара у поставщика
    /// </summary>
    public class SupplierProduct
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Поставщик
        /// </summary>
        public Supplier Supplier { get; set; }

        /// <summary>
        ///     Идентификатор поставщика
        /// </summary>
        public string SupplierId { get; set; }

        /// <summary>
        ///     Номенклатура
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        ///     Идентификатор Номенклатура
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        ///     Код в прайсе поставщика (Артикул)
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Наименование товара у поставщика
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Цена 1
        /// </summary>
        public decimal Price1 { get; set; }

        /// <summary>
        ///     Цена 2
        /// </summary>
        public decimal? Price2 { get; set; }

        /// <summary>
        ///     Цена 3
        /// </summary>
        public decimal? Price3 { get; set; }

        /// <summary>
        ///     Доступное количество
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        ///     Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Список свойств (характеристик)
        /// </summary>
        public List<CategoryPropertyValue> Properties { get; set; } = new List<CategoryPropertyValue>();

        /// <summary>
        ///     Версия
        /// </summary>
        public string Version { get; set; }
    }
}