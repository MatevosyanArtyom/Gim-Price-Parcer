using System;
using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities;

namespace Gim.PriceParser.WebApi.Models.Supplier
{
    public abstract class SupplierBase
    {
        [Required]
        public string Name { get; set; }

        public string Inn { get; set; }
        public string Version { get; set; }
        public DateTime Modified { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsArchived { get; set; }
        public EntityStatus Status { get; set; }
    }
}