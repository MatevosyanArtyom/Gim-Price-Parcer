using System.Collections.Generic;

namespace Gim.PriceParser.Bll.Common.Entities.PriceListItem
{
    public class PriceListItemPropertyFilter
    {
        public string PriceListItemId { get; set; }
        public List<string> PriceListItemsIds { get; set; }
        public string PropertyKey { get; set; }
        public PriceListItemStatus? Status { get; set; }
    }
}