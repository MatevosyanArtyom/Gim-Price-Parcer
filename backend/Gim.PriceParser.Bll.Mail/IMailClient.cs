using System.Threading.Tasks;

namespace Gim.PriceParser.Bll.Mail
{
    public interface IMailClient
    {
        Task ReceiveMessagesAsync();
        Task SendMessageAsync(string email, string subject, string text);
    }
}