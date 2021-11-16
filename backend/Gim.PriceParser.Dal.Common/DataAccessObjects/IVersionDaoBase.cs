using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    public interface IVersionDaoBase<T> : IDaoBase<T>
    {
        Task<GetAllResult<EntityVersion<T>>> GetVersions(string id, int page, int pageSize);
        Task<T> RestoreVersion(string versionId);
    }
}