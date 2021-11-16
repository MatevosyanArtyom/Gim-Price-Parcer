using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Users;
using Gim.PriceParser.Bll.Common.Sort;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    public interface IUserDao : IDaoBase<GimUser>, IArchivableDao
    {
        Task<GetAllResult<GimUser>> GetManyAsync(GimUserFilter filter, SortParams sort, int page, int pageSize);
        Task<GimUser> GetOneAsync(GimUserFilter filter);

        /// <summary>
        ///     Возвращает пользователя с точным совпадением e-mail. Используется при аутентификации
        /// </summary>
        /// <param name="email">e-mail</param>
        /// <returns></returns>
        Task<GimUser> GetOneByEmailAsync(string email);

        Task<GimUser> SetPasswordTokenAsync(string id);
    }
}