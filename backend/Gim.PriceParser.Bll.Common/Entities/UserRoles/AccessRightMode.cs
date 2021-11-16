namespace Gim.PriceParser.Bll.Common.Entities.UserRoles
{
    /// <summary>
    ///     Права роли
    /// </summary>
    public class AccessRightMode
    {
        /// <summary>
        ///     Разрешен просмотр только 'своих'
        /// </summary>
        public bool ReadSelf { get; set; }

        /// <summary>
        ///     Разрешен просмотр
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        ///     Разрешено редактирование только 'своих'
        /// </summary>
        public bool EditSelf { get; set; }

        /// <summary>
        ///     Разрешено редактирование
        /// </summary>
        public bool Edit { get; set; }

        /// <summary>
        ///     Полный доступ
        /// </summary>
        public bool Full { get; set; }

        /// <summary>
        ///     Доступно создание характеристик при загрузке прайс-листа
        ///     TODO: вынести в дочерний класс
        /// </summary>
        public bool CreateProperties { get; set; }
    }
}