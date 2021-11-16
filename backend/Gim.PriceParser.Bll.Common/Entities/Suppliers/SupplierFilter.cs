using System;

namespace Gim.PriceParser.Bll.Common.Entities.Suppliers
{
    public class SupplierFilter
    {
        public long? SeqId { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Inn { get; set; }
        public string UserId { get; set; }
        public EntityStatus? Status { get; set; }
        public bool? IsArchived { get; set; }
    }
}