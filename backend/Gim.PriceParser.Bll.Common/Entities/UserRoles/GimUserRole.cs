using System;

namespace Gim.PriceParser.Bll.Common.Entities.UserRoles
{
    /// <summary>
    ///     Роль пользователя
    /// </summary>
    public class GimUserRole
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
        ///     Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Признак главного администратора
        /// </summary>
        public bool IsMainAdmin { get; set; }

        /// <summary>
        ///     Количество пользователей
        /// </summary>
        public int UsersCount { get; set; }

        /// <summary>
        ///     Дата создания
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     Матрица прав доступа
        /// </summary>
        public AccessRights AccessRights { get; set; } = new AccessRights();

        /// <summary>
        ///     Признак архивного
        /// </summary>
        public bool IsArchived { get; set; }
    }
}