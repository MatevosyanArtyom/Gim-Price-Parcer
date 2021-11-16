namespace Gim.PriceParser.Bll.Common
{
    /// <summary>
    ///     Фильтр по признаку архивности
    /// </summary>
    public enum ArchivedFilter
    {
        Unknown = 0,

        /// <summary>
        ///     Только действующие
        /// </summary>
        OnlyActive = 1,

        /// <summary>
        ///     Только архивные
        /// </summary>
        OnlyArchived = 2
    }
}