namespace Gim.PriceParser.Bll.Common.Entities
{
    /// <summary>
    ///     Производитель товаров
    /// </summary>
    public class Manufacturer
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
        ///     Описание
        /// </summary>
        public string Description { get; set; }
    }
}