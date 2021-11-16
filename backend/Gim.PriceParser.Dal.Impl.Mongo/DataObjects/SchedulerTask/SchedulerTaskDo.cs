using Gim.PriceParser.Bll.Common.Entities.SchedulerTasks;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Bson;

namespace Gim.PriceParser.Dal.Impl.Mongo.DataObjects.SchedulerTask
{
    internal class SchedulerTaskDo : IEntityWithIdDo, IEntityWithVersionDo
    {
        public string Name { get; set; }
        public ObjectId SupplierId { get; set; }
        public IntegrationMethod IntegrationMethod { get; set; }
        public bool RequestRequired { get; set; }
        public SchedulerTaskStartBy StartBy { get; set; }
        public string Emails { get; set; }
        public string Schedule { get; set; }
        public string Script { get; set; }
        public SchedulerTaskStatus Status { get; set; }
        public ObjectId Id { get; set; }
        public long SeqId { get; set; }
        public ObjectId Version { get; set; }
    }
}