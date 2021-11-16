using Microsoft.Extensions.DependencyInjection;

namespace Gim.PriceParser.Bll.Mail
{
    public static class MailModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMailClient, MailClient>();
        }
    }
}