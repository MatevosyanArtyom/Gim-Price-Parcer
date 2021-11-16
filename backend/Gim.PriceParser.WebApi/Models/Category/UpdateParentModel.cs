using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Category
{
    public class MergeCategoryModel
    {
        /// <summary>
        ///     Идентификатор категории, которую объединить
        /// </summary>
        [Required]
        public string FromId { get; set; }

        /// <summary>
        ///     Идентификатор категории, с которой объединить
        /// </summary>
        [Required]
        public string ToId { get; set; }
    }
}