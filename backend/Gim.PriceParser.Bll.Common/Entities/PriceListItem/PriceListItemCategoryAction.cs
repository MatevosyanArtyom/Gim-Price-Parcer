using System;

namespace Gim.PriceParser.Bll.Common.Entities.PriceListItem
{
    /// <summary>
    ///     Действие, которое будет применено к категории при загрузке строки в БД
    /// </summary>
    public enum PriceListItemCategoryAction
    {
        Unknown = 0,

        /// <summary>
        ///     Добавить в список маппингов (синонимов)
        /// </summary>
        MapTo = 2
    }
}