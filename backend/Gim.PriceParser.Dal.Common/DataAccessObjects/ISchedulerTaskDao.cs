using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.SchedulerTasks;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    /// <summary>
    ///     Интерфейс для работы с задачами планировщика
    /// </summary>
    public interface ISchedulerTaskDao : IDaoBase<SchedulerTask>
    {
        /// <summary>
        ///     Получает элементы постранично с фильтрацией
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <returns></returns>
        Task<GetAllResult<SchedulerTask>> GetManyAsync(SchedulerTaskFilter filter, int page = 0, int pageSize = 0);
    }
}