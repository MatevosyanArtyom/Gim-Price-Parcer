namespace Gim.PriceParser.Bll.Common.Entities.PriceListItem
{
    /// <summary>
    ///     Первичная модель строки прайс-листа, в которую парсится источник
    /// </summary>
    public class PriceListItemBase
    {
        /// <summary>
        ///     Код в прайсе поставщика (Артикул)
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Наименование в прайсе поставщика
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///     Наименование категории 1 в прайсе поставщика
        /// </summary>
        public string Category1Name { get; set; }

        /// <summary>
        ///     Наименование категории 2 в прайсе поставщика
        /// </summary>
        public string Category2Name { get; set; }

        /// <summary>
        ///     Наименование категории 3 в прайсе поставщика
        /// </summary>
        public string Category3Name { get; set; }

        /// <summary>
        ///     Наименование категории 4 в прайсе поставщика
        /// </summary>
        public string Category4Name { get; set; }

        /// <summary>
        ///     Наименование категории 5 в прайсе поставщика
        /// </summary>
        public string Category5Name { get; set; }

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
        ///     Количество
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        ///     Описание
        /// </summary>
        public string Description { get; set; }
    }
}