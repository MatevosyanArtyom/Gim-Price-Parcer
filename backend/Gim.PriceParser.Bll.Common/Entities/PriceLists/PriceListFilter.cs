using System;

namespace Gim.PriceParser.Bll.Common.Entities.PriceLists
{
    public class PriceListFilter
    {
        public long? SeqId { get; set; }
        public DateTime? ParsedFrom { get; set; }
        public DateTime? ParsedTo { get; set; }
        public string SupplierId { get; set; }
        public RulesSource? RulesSource { get; set; }
        public PriceListStatus? Status { get; set; }

        /// <summary>
        ///     Исключить элементы с указанным статусом из результата выборки
        /// </summary>
        public PriceListStatus? ExceptStatus { get; set; }
    }
}