using System;
using Gim.PriceParser.Bll.Common.Entities.UserRoles;

namespace Gim.PriceParser.Bll.Common.Entities.Users
{
    /// <summary>
    ///     Пользователь
    /// </summary>
    public class GimUser
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Числовой идентификатор
        /// </summary>
        public long SeqId { get; set; }

        /// <summary>
        ///     Адрес эл.почты пользователя. Является именем пользователя при логине
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     ФИО
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        ///     Должность
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        ///     Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     Роль пользователя
        /// </summary>
        public GimUserRole Role { get; set; }

        /// <summary>
        ///     Идентификатор роли
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        ///     Доступ к разделу поставщиков
        /// </summary>
        public bool HasSuppliersAccess { get; set; }

        /// <summary>
        ///     Полный доступ к системе (кроме раздела пользователей)
        /// </summary>
        public bool HasFullAccess { get; set; }

        /// <summary>
        ///     Доступ к разделу пользователей
        /// </summary>
        public bool HasUsersAccess { get; set; }

        /// <summary>
        ///     Токен смены пароля
        /// </summary>
        public string ChangePasswordToken { get; set; }

        /// <summary>
        ///     Дата создания
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     Признак архивного
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        ///     Статус
        /// </summary>
        public GimUserStatus Status { get; set; }
    }
}