using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities.Suppliers;

namespace Gim.PriceParser.WebApi.Models.Supplier
{
    public abstract class SupplierFull : SupplierBase
    {
        public FiasEntity Region { get; set; } = new FiasEntity();
        public FiasEntity City { get; set; } = new FiasEntity();
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LegalAddress { get; set; }
        public string OfficeAddress { get; set; }
        public BankDetails BankDetails { get; set; }

        [Required]
        public List<ContactPersonDto> ContactPersons { get; set; } = new List<ContactPersonDto>();

        public bool LargeWholesale { get; set; }
        public bool SmallWholesale { get; set; }
        public bool Retail { get; set; }
        public bool Installment { get; set; }
        public bool Credit { get; set; }
        public bool Deposit { get; set; }
        public bool TransferForSale { get; set; }
        public bool HasShowroom { get; set; }
        public bool WorkWithIndividuals { get; set; }

        public bool PaidPartnership { get; set; }
        public bool Dropshipping { get; set; }
        public int MinimumPurchase { get; set; }
    }
}