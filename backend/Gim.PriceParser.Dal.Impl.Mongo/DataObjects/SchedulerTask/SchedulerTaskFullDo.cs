using System.Collections.Generic;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Supplier;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SchedulerTask
{
    internal class SchedulerTaskFullDo : SchedulerTaskDo
    {
        public List<SupplierDo> Suppliers { get; set; }
    }
}