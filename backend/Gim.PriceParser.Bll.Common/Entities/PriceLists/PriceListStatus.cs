namespace Gim.PriceParser.Bll.Common.Entities.PriceLists
{
    /// <summary>
    ///     Статус обработки прайс-листа
    /// </summary>
    public enum PriceListStatus
    {
        Unknown = 0,

        /// <summary>
        ///     Еще не обрабатывался (в очереди)
        /// </summary>
        InQueue = 1,

        /// <summary>
        ///     В обработке
        /// </summary>
        Processing = 2,

        /// <summary>
        ///     Есть ошибки
        /// </summary>
        Errors = 3,

        /// <summary>
        ///     Готово к загрузке (ошибок нет)
        /// </summary>
        Ready = 4,

        /// <summary>
        ///     Загружено в БД
        /// </summary>
        Committed = 5,

        /// <summary>
        ///     Невозможно закончить обработку прайс-листа
        /// </summary>
        Failed = 6
    }
}
