using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.SchedulerTask
{
    public class SchedulerTaskEdit : SchedulerTaskBase
    {
        [Required]
        public string Id { get; set; }
    }
}