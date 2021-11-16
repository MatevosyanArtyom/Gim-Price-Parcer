using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;

namespace Gim.PriceParser.WebApi.Models.PriceListItem
{
    public class PriceListItemLookup
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }

        [Required]
        public string PriceListId { get; set; }

        public string Code { get; set; }
        public string Product { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public bool HasSynonyms { get; set; }
        public string Category1Name { get; set; }
        public string Category1Id { get; set; }
        public PriceListItemStatus Category1Status { get; set; }
        public PriceListItemCategoryAction Category1Action { get; set; }
        public string MapTo1Id { get; set; }
        public string Category2Name { get; set; }
        public string Category2Id { get; set; }
        public PriceListItemStatus Category2Status { get; set; }
        public PriceListItemCategoryAction Category2Action { get; set; }
        public string MapTo2Id { get; set; }
        public string Category3Name { get; set; }
        public string Category3Id { get; set; }
        public PriceListItemStatus Category3Status { get; set; }
        public PriceListItemCategoryAction Category3Action { get; set; }
        public string MapTo3Id { get; set; }
        public string Category4Name { get; set; }
        public string Category4Id { get; set; }
        public PriceListItemStatus Category4Status { get; set; }
        public PriceListItemCategoryAction Category4Action { get; set; }
        public string MapTo4Id { get; set; }
        public string Category5Name { get; set; }
        public string Category5Id { get; set; }
        public PriceListItemStatus Category5Status { get; set; }
        public PriceListItemCategoryAction Category5Action { get; set; }
        public string MapTo5Id { get; set; }
        public decimal? Price1 { get; set; }
        public decimal? Price2 { get; set; }
        public decimal? Price3 { get; set; }
        public decimal? Quantity { get; set; }
        public string Description { get; set; }

        [Required]
        public PriceListItemAction NameAction { get; set; }

        [Required]
        public PriceListItemStatus NameStatus { get; set; }

        [Required]
        public PriceListItemStatus Status { get; set; }

        public bool Skip { get; set; }
    }
}