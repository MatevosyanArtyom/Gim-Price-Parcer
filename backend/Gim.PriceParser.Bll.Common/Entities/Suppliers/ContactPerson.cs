namespace Gim.PriceParser.Bll.Common.Entities.Suppliers
{
    /// <summary>
    ///     Контактное лицо поставщика
    /// </summary>
    public class ContactPerson
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     Skype
        /// </summary>
        public string Skype { get; set; }

        /// <summary>
        ///     Доступен в Telegram
        /// </summary>
        public bool HasTelegram { get; set; }

        /// <summary>
        ///     Доступен в Viber
        /// </summary>
        public bool HasViber { get; set; }

        /// <summary>
        ///     Доступен в WhatsApp
        /// </summary>
        public bool HasWhatsApp { get; set; }

        /// <summary>
        ///     Время доступности
        /// </summary>
        public string Availability { get; set; }
    }
}