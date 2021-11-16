namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier
{
    /// <summary>
    ///     Данные бансковского счета
    /// </summary>
    internal class BankDetailsDo
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