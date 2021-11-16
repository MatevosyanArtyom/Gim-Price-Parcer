using Gim.PriceParser.Processor.RuntimeCompiler;
using Microsoft.Extensions.DependencyInjection;

namespace Gim.PriceParser.Processor
{
    public static class ProcessorModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRuntimeCompiler, RoslynCompiler>();
            services.AddTransient<IProcessorClient, ProcessorClient>();
        }
    }
}