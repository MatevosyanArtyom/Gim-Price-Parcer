using System.Threading.Tasks;

namespace Gim.PriceParser.Dal.Common
{
    public interface IDbSeeder
    {
        Task Seed();
    }
}