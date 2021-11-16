using System.Collections.Generic;

namespace Gim.PriceParser.WebApi.Models.SchedulerTask
{
    public class EmitResultDto
    {
        public bool Success { get; set; }
        public List<DiagnosticDto> Diagnostics { get; set; }
    }
}