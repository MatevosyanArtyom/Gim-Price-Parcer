namespace Gim.PriceParser.Bll.Common.Entities.PriceListItem
{
    public class PriceListItemFilter
    {
        public string PriceListId { get; set; }
        public string Code { get; set; }
        public string CategoryName { get; set; }
        public string Category1Name { get; set; }
        public string Category2Name { get; set; }
        public string Category3Name { get; set; }
        public string Category4Name { get; set; }
        public string Category5Name { get; set; }
        public string ProductNameEq { get; set; }
        public string ProductNameRegEx { get; set; }
        public decimal? Price1 { get; set; }
        public decimal? Price2 { get; set; }
        public decimal? Price3 { get; set; }
        public PriceListItemStatus? Status { get; set; }
        public bool? Skip { get; set; }

        /// <summary>
        ///     Для получения необработанных строк (статус - ошибка, и действие - неустановлено)
        /// </summary>
        public bool? UnprocessedItemsOnly { get; set; }

        /// <summary>
        ///     Для получения обработанных строк, но со статусом Error.
        ///     Используется для получения элементов, которым нужно установить статус Fixed,
        ///     после какого-либо действия
        /// </summary>
        public bool? ProcessedItemsOnly { get; set; }

        /// <summary>
        ///     Для получения непропущенных (не удаленных) строк с незаполненным артикулом
        /// </summary>
        public bool? UnprocessedCodeError { get; set; }

        /// <summary>
        ///     Для получения непропущенных (не удаленных) строк с незаполненным наименованием
        /// </summary>
        public bool? UnprocessedNameErrors { get; set; }

        /// <summary>
        ///     Для получения непропущенных (не удаленных) строк с незаполненной ценой 1
        /// </summary>
        public bool? UnprocessedPriceError { get; set; }
    }
}