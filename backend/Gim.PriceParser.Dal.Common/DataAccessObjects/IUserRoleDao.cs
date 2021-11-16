using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.UserRoles;
using Gim.PriceParser.Bll.Common.Sort;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    /// <summary>
    ///     Интерфейс для работы с ролями
    /// </summary>
    public interface IUserRoleDao : IDaoBase<GimUserRole>, IArchivableDao
    {
        Task<GetAllResult<GimUserRole>> GetManyAsync(GimUserRoleFilter filter, SortParams sort, int page, int pageSize);
        Task<GimUserRole> GetMainAdminAsync();
    }
}