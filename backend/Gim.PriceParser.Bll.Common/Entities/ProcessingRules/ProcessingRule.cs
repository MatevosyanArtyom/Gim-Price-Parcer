using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.Bll.Common.Entities.Suppliers;

namespace Gim.PriceParser.Bll.Common.Entities.ProcessingRules
{
    /// <summary>
    ///     Правила обработки прайс-листа
    /// </summary>
    public class ProcessingRule
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
        ///     Поставщик
        /// </summary>
        public Supplier Supplier { get; set; }

        /// <summary>
        ///     Идентификатор поставщика
        /// </summary>
        public string SupplierId { get; set; }

        /// <summary>
        ///     Источник правил для обработки прайс-листа
        /// </summary>
        public RulesSource RulesSource { get; set; }

        /// <summary>
        ///     Код правил
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Признак архивного
        /// </summary>
        public bool IsArchived { get; set; }
    }
}