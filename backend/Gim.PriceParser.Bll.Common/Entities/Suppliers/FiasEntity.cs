namespace Gim.PriceParser.Bll.Common.Entities.Suppliers
{
    /// <summary>
    ///     Элемент адреса с данными ФИАС
    /// </summary>
    public class FiasEntity
    {
        /// <summary>
        ///     Данные ФИАС
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        ///     Идентификатор элемента в ФИАС
        /// </summary>
        public string FiasId { get; set; }

        /// <summary>
        ///     Наименование элемента
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///     Полное наименование элемента
        /// </summary>
        public string UnrestrictedValue { get; set; }
    }
}