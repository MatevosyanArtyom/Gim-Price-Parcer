using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Util
{
    public class TreeItem<T>
    {
        [Required]
        public T Item { get; set; }

        [Required]
        public IEnumerable<TreeItem<T>> Children { get; set; } = new List<TreeItem<T>>();
    }
}