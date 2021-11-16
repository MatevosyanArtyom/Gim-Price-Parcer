using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.ProcessingRule;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SchedulerTask;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.User;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.PriceList
{
    internal class PriceListFullDo : PriceListDo
    {
        public List<SupplierDo> Suppliers { get; set; }
        public List<SchedulerTaskDo> SchedulerTasks { get; set; }
        public List<ProcessingRuleDo> ProcessingRules { get; set; }
        public List<UserDo> Authors { get; set; }
    }
}