using System;
using System.Collections.Generic;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Suppliers;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier
{
    internal class SupplierDo : IEntityWithIdDo, IEntityWithVersionDo, IEntityArchivableDo
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Inn { get; set; }
        public FiasEntity Region { get; set; }
        public FiasEntity City { get; set; }
        public BankDetailsDo BankDetails { get; set; }
        public List<ContactPersonDo> ContactPersons { get; set; } = new List<ContactPersonDo>();
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LegalAddress { get; set; }
        public string OfficeAddress { get; set; }
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
        public ObjectId UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public EntityStatus Status { get; set; }
        public bool IsArchived { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
        public ObjectId Version { get; set; }
    }
}