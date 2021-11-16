using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities.SchedulerTasks;

namespace Gim.PriceParser.WebApi.Models.SchedulerTask
{
    public class SchedulerTaskBase
    {
        [Required]
        public string Name { get; set; }

        public string Supplier { get; set; }
        public IntegrationMethod IntegrationMethod { get; set; }
        public bool RequestRequired { get; set; }
        public SchedulerTaskStartBy StartBy { get; set; }
        public string Emails { get; set; }
        public string Schedule { get; set; }
        public string Script { get; set; }
        public SchedulerTaskStatus Status { get; set; }
        public string Version { get; set; }
    }
}