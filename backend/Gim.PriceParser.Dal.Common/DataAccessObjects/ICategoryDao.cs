using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;
using Gim.PriceParser.Bll.Common.Entities.Categories;
using Gim.PriceParser.Bll.Common.Entities.PriceListItem;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    /// <summary>
    ///     Интерфейс для работы с категориями
    /// </summary>
    public interface ICategoryDao : IDaoWithVersionsBase<Category>
    {
        /// <summary>
        ///     Изменяет родителя у категории
        /// </summary>
        /// <param name="id">Идентификатор категории, у которой меняется родитель</param>
        /// <param name="newParentId">Идентификатор нового родителя</param>
        /// <returns></returns>
        Task<Category> UpdateParentAsync(string id, string newParentId);

        /// <summary>
        ///     Изменяет маппинги для категории
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <param name="mappings">Список маппингов (вариантов написания наименования)</param>
        /// <returns></returns>
        Task<Category> UpdateMappingsAsync(string id, List<CategoryMappingItem> mappings);

        /// <summary>
        ///     Объединяет категории
        /// </summary>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <returns></returns>
        Task<Category> MergeOneAsync(string fromId, string toId);

        /// <summary>
        ///     Перемещает категорию
        /// </summary>
        /// <param name="id">Идентификатор перемещаемой категориии</param>
        /// <param name="afterId">Идентификатор категории, после которой необходимо поместить перемещаемую</param>
        /// <returns></returns>
        Task<Category> MoveOneAsync(string id, string afterId);

        /// <summary>
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<Category>> FindOneAsync(string filter);

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<List<PriceListItemMatched>> AddAbsentItemsAsync(List<PriceListItemMatched> items);

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<List<PriceListItemMatched>> MatchItemsAsync(List<PriceListItemMatched> items);

        /// <summary>
        ///     Возвращает дочерние элементы первого уровня (дети) для категории
        /// </summary>
        /// <param name="parentId">Идентификатор категории</param>
        /// <returns></returns>
        Task<List<Category>> GetChildrenAsync(string parentId);

        /// <summary>
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<Category>> GetChildrenFlattenAsync(CategoryFilter filter);

        /// <summary>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<GetAllResult<Category>> GetVersions(int page, int pageSize);

        /// <summary>
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        Task RestoreVersion(string version);

        Task DeleteManyAsync();
    }
}