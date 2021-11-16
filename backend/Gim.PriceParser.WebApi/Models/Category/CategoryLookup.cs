using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Category
{
    public class CategoryLookup : CategoryBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string Parent { get; set; }

        [Required]
        public string RootParent { get; set; }

        [Required]
        public int Position { get; set; }

        public bool HasChildren { get; set; }
    }
}