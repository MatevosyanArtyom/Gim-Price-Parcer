using System.Threading.Tasks;

namespace Gim.PriceParser.Dal.Common
{
    public interface IDbConfigurer
    {
        Task ConfigureIndexes();
    }
}