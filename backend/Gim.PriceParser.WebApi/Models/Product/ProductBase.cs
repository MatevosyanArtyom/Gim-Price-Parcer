using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities;

namespace Gim.PriceParser.WebApi.Models.Product
{
    public class ProductBase
    {
        [Required]
        public string Name { get; set; }

        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string Category3 { get; set; }
        public string Category4 { get; set; }
        public string Category5 { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Supplier { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        public string Description { get; set; }
        public decimal? PriceFrom { get; set; }
        public EntityStatus Status { get; set; }
        public string Version { get; set; }
    }
}