using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Suppliers;
using Gim.PriceParser.Bll.Common.Sort;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    /// <summary>
    ///     Интерфейс для работы с поставщиками
    /// </summary>
    public interface ISupplierDao : IVersionDaoBase<Supplier>, IArchivableDao
    {
        Task<GetAllResult<Supplier>> GetManyAsync(SupplierFilter filter, SortParams sort, int page, int pageSize);
        Task<long> CountAsync(SupplierFilter filter);
    }
}