namespace Gim.PriceParser.Bll.Common.Entities.Suppliers
{
    /// <summary>
    ///     Данные бансковского счета
    /// </summary>
    public class BankDetails
    {
        /// <summary>
        ///     БИК
        /// </summary>
        public string Rcbic { get; set; }

        /// <summary>
        ///     Номер счета
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        ///     Корр. счет
        /// </summary>
        public string CorrespondentAccount { get; set; }
    }
}