using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.ProcessingRules;
using Gim.PriceParser.Bll.Common.Sort;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    /// <summary>
    ///     Интерфейс для работы с правилами обработки прайс-листов
    /// </summary>
    public interface IProcessingRuleDao : IDaoBase<ProcessingRule>, IArchivableDao
    {
        Task<GetAllResult<ProcessingRule>> GetManyAsync(ProcessingRuleFilter filter, SortParams sort, int page,
            int pageSize);
    }
}