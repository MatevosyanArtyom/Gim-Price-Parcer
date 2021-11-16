using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.ProcessingRule
{
    internal class ProcessingRuleDo : IEntityWithIdDo, IEntityArchivableDo
    {
        public string Name { get; set; }
        public ObjectId SupplierId { get; set; }
        public RulesSource RulesSource { get; set; }
        public string Code { get; set; }
        public bool IsArchived { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
    }
}