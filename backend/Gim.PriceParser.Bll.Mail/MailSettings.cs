namespace Gim.PriceParser.Bll.Mail
{
    public class MailSettings
    {
        public string ImapHost { get; set; }
        public int ImapPort { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        /// <summary>
        ///     Адрес для ссылки смены пароля
        /// </summary>
        public string HostName { get; set; }
    }
}