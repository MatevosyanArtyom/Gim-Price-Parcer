namespace Gim.PriceParser.Bll.Common.Entities.PriceListItem
{
    /// <summary>
    ///     Действие, которое будет применено при загрузке строки в БД
    /// </summary>
    public enum PriceListItemAction
    {
        Unknown = 0,

        /// <summary>
        ///     Создать новый элемент (элементы)
        /// </summary>
        CreateNew = 1,

        /// <summary>
        ///     Оставить прежнее значение (для наименования и свойств)
        /// </summary>
        LeaveOld = 2,

        /// <summary>
        ///     Применить новое значение (для наименования и свойств)
        /// </summary>
        ApplyNew = 4
    }
}