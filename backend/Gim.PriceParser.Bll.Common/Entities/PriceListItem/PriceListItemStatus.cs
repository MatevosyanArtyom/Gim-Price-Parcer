namespace Gim.PriceParser.Bll.Common.Entities.PriceListItem
{
    /// <summary>
    ///     Статус строки прайс-листа
    /// </summary>
    public enum PriceListItemStatus
    {
        Unknown = 0,

        /// <summary>
        ///     Есть ошибки
        /// </summary>
        Error = 1,

        /// <summary>
        ///     Исправленная ошибка
        /// </summary>
        Fixed = 2,

        /// <summary>
        ///     Нет ошибок
        /// </summary>
        Ok = 3
    }
}