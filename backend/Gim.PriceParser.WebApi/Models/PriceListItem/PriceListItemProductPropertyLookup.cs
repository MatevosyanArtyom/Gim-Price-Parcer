using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;

namespace Gim.PriceParser.WebApi.Models.PriceListItem
{
    public class PriceListItemProductPropertyLookup
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }

        [Required]
        public string PropertyKey { get; set; }

        /// <summary>
        ///     Наименование свойства
        /// </summary>
        public string Property { get; set; }

        public string PropertyId { get; set; }

        public string PropertyValue { get; set; }
        public string ValueId { get; set; }

        /// <summary>
        ///     Прежнее значение свойства
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///     Значение свойства у элемента номенклатуры
        /// </summary>
        public string ProductValue { get; set; }

        [Required]
        public PriceListItemAction Action { get; set; }

        [Required]
        public PriceListItemStatus Status { get; set; }
    }
}