namespace Gim.PriceParser.Bll.Common.Entities.Users
{
    /// <summary>
    ///     Статусы пользователей
    /// </summary>
    public enum GimUserStatus
    {
        Unknown = 0,

        /// <summary>
        ///     Новый (неактивный). Присваивается новым пользователям до смены ими пароля
        /// </summary>
        New = 1,

        /// <summary>
        ///     Активный
        /// </summary>
        Active = 2,

        /// <summary>
        ///     Заблокирован
        /// </summary>
        Blocked = 3,

        /// <summary>
        ///     Заблокирован системой
        /// </summary>
        SystemBlocked = 4
    }
}