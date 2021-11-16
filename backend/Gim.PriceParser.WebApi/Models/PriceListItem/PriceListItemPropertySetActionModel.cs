using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;

namespace Gim.PriceParser.WebApi.Models.PriceListItem
{
    public class PriceListItemPropertySetActionModel
    {
        [Required]
        public string PriceListId { get; set; }

        public string PropertyKey { get; set; }

        [Required]
        public PriceListItemAction Action { get; set; }
    }
}