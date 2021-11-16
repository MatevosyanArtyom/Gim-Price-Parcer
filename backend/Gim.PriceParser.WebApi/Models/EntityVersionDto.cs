using System;
using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models
{
    public class EntityVersionDto<T>
    {
        [Required]
        public T Entity { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public long SeqId { get; set; }
    }
}