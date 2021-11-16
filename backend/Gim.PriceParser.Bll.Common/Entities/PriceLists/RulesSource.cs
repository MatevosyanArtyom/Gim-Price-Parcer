namespace Gim.PriceParser.Bll.Common.Entities.PriceLists
{
    /// <summary>
    ///     Источник правил для прайс-листа
    /// </summary>
    public enum RulesSource
    {
        Unknown = 0,

        /// <summary>
        ///     Код
        /// </summary>
        Code = 1,

        /// <summary>
        ///     Файл
        /// </summary>
        File = 2
    }
}