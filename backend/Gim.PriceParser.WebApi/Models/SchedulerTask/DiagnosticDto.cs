using Microsoft.CodeAnalysis;

namespace Gim.PriceParser.WebApi.Models.SchedulerTask
{
    public class DiagnosticDto
    {
        public DiagnosticSeverity Severity { get; set; }
        public string Message { get; set; }
    }
}