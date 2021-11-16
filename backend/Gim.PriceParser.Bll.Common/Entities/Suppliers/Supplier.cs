using System;
using System.Collections.Generic;
using Gim.PriceParser.Bll.Common.Entities.Users;

namespace Gim.PriceParser.Bll.Common.Entities.Suppliers
{
    /// <summary>
    ///     Поставщик
    /// </summary>
    public class Supplier
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
        ///     Владелец
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        ///     ИНН
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        ///     Область
        /// </summary>
        public FiasEntity Region { get; set; } = new FiasEntity();

        /// <summary>
        ///     Город
        /// </summary>
        public FiasEntity City { get; set; } = new FiasEntity();

        /// <summary>
        ///     Данные банковского счета
        /// </summary>
        public BankDetails BankDetails { get; set; }

        /// <summary>
        ///     Контактные лица
        /// </summary>
        public List<ContactPerson> ContactPersons { get; set; } = new List<ContactPerson>();

        /// <summary>
        ///     Количество товаров
        /// </summary>
        public long ProductsCount { get; set; }

        /// <summary>
        ///     Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     Фактический адрес
        /// </summary>
        public string LegalAddress { get; set; }

        /// <summary>
        ///     Юридический адрес
        /// </summary>
        public string OfficeAddress { get; set; }

        /// <summary>
        ///     Крупный опт
        /// </summary>
        public bool LargeWholesale { get; set; }

        /// <summary>
        ///     Мелкий опт
        /// </summary>
        public bool SmallWholesale { get; set; }

        /// <summary>
        ///     Поштучно
        /// </summary>
        public bool Retail { get; set; }

        /// <summary>
        ///     Рассрочка
        /// </summary>
        public bool Installment { get; set; }

        /// <summary>
        ///     Кредит
        /// </summary>
        public bool Credit { get; set; }

        /// <summary>
        ///     Депозит
        /// </summary>
        public bool Deposit { get; set; }

        /// <summary>
        ///     Передача на реализацию
        /// </summary>
        public bool TransferForSale { get; set; }

        /// <summary>
        ///     Есть шоурум
        /// </summary>
        public bool HasShowroom { get; set; }

        /// <summary>
        ///     Работа с ФЛ
        /// </summary>
        public bool WorkWithIndividuals { get; set; }

        /// <summary>
        ///     Партнерство платное
        /// </summary>
        public bool PaidPartnership { get; set; }

        /// <summary>
        ///     Дропшиппинг
        /// </summary>
        public bool Dropshipping { get; set; }

        /// <summary>
        ///     Минимальная закупка
        /// </summary>
        public int MinimumPurchase { get; set; }

        /// <summary>
        ///     Ответственный пользователь
        /// </summary>
        public GimUser User { get; set; }

        /// <summary>
        ///     Идентификатор ответственного пользователя
        /// </summary>
        public string UserId { get; set; }

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
        public EntityStatus Status { get; set; }

        /// <summary>
        ///     Версия
        /// </summary>
        public string Version { get; set; }
    }
}