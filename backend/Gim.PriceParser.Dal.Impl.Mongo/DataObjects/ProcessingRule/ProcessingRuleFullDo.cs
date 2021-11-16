using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.ProcessingRule
{
    internal class ProcessingRuleFullDo : ProcessingRuleDo
    {
        public List<SupplierDo> Suppliers { get; set; }
    }
}