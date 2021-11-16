using System;
using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.SchedulerTask
{
    public class SchedulerTaskFull : SchedulerTaskBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public DateTime Modified { get; set; }
    }
}