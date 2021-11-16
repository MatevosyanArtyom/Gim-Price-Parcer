using System;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceList
{
    internal class PriceListDo : IEntityWithIdDo
    {
        public ObjectId SupplierId { get; set; }
        public ObjectId SchedulerTaskId { get; set; }
        public ObjectId ProcessingRuleId { get; set; }
        //public RulesSource RulesSource { get; set; }
        //public string Code { get; set; }
        //public GimFile CodeFile { get; set; }
        public GimFile PriceListFile { get; set; }
        public bool HasUnprocessedCodeErrors { get; set; }
        public bool HasUnprocessedNameErrors { get; set; }
        public bool HasUnprocessedPriceErrors { get; set; }
        public bool HasUnprocessedErrors { get; set; }
        public bool HasPropertiesErrors { get; set; }
        public bool CreateProperties { get; set; }
        public PriceListStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public ObjectId AuthorId { get; set; }
        public DateTime StatusDate { get; set; }
        public DateTime? ParsedDate { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}