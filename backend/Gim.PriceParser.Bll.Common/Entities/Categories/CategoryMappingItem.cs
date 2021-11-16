using System;

namespace Gim.PriceParser.Bll.Common.Entities.Categories
{
    /// <summary>
    ///     Элемент коллекции маппингов для категории
    /// </summary>
    public class CategoryMappingItem
    {
        /// <summary>
        ///     Наименование (вариант написания наименования)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Дата добавления
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}