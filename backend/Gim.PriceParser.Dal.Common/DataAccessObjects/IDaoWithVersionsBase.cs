using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    public interface IDaoWithVersionsBase<T> : IDaoBase<T>
    {
        /// <summary>
        ///     Получает версии элемента постранично
        /// </summary>
        /// <param name="id">Идентификатор элемента</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <returns></returns>
        Task<GetAllResult<T>> GetVersions(string id, int page, int pageSize);

        /// <summary>
        ///     Восстанавливает элемент на указанную версию
        /// </summary>
        /// <param name="id">Идентификатор элемента</param>
        /// <param name="version">Версия элемента</param>
        /// <returns></returns>
        Task<T> RestoreVersion(string id, string version);
    }
}