namespace Gim.PriceParser.Bll.Common.Entities.PriceListItem
{
    /// <summary>
    ///     Ошибки элемента прайс-листа
    /// </summary>
    public enum PriceListItemError
    {
        /// <summary>
        ///     Нет артикула
        /// </summary>
        NoCode = 1,

        /// <summary>
        ///     Нет наименования
        /// </summary>
        NoProductName = 2,

        /// <summary>
        ///     Нет цены
        /// </summary>
        NoPrice = 3,

        /// <summary>
        ///     Новые значения полей описания
        /// </summary>
        PropertiesMismatch = 4,

        /// <summary>
        ///     Найдены похожие
        /// </summary>
        Similarfound = 5,

        /// <summary>
        ///     Ошибка уровня каталога
        /// </summary>
        CategoryError = 6
    }
}
