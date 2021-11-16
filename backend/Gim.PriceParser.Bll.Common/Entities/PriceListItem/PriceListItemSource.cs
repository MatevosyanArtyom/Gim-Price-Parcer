using System.Collections.Generic;

namespace Gim.PriceParser.Bll.Common.Entities.PriceListItem
{
    /// <summary>
    ///     Первичная модель строки прайс-листа, в которую парсится источник
    /// </summary>
    public class PriceListItemSource : PriceListItemBase
    {
        /// <summary>
        ///     Набор характеристик и их значений
        /// </summary>
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        /// <summary>
        ///     Массив ссылок на изображения
        /// </summary>
        public List<string> Images { get; set; } = new List<string>();
    }
}