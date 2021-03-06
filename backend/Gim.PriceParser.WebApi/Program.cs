using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Gim.PriceParser.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(Startup.Configuration)
                .UseStartup<Startup>();
        }
    }   
}
