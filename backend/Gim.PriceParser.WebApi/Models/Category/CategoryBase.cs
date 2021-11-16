using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Categories;

namespace Gim.PriceParser.WebApi.Models.Category
{
    public class CategoryBase
    {
        public long ProductsCount { get; set; }
        public string Version { get; set; }
        public DateTime Modified { get; set; }
        public EntityStatus Status { get; set; }

        [Required]
        public IEnumerable<CategoryMappingItem> Mappings { get; set; } = new List<CategoryMappingItem>();

        [Required]
        public string Name { get; set; }
    }
}