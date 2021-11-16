using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities.Categories;

namespace Gim.PriceParser.Bll.Services.Categories
{
    public interface ICategoryService
    {
        /// <summary>
        ///     Возвращает дочерние категории в виде списка
        /// </summary>
        /// <param name="ids">Идентификаторы категорий</param>
        /// <param name="parents">Идентификаторы родительских категорий</param>
        /// <param name="includeRoot">Включать корень</param>
        /// <returns></returns>
        Task<List<Category>> GetChildrenFlattenAsync(List<string> ids, List<string> parents, bool includeRoot);

        /// <summary>
        ///     Объединяет категории
        /// </summary>
        /// <param name="fromId">Идентификатор исходной категории</param>
        /// <param name="toId">Идентификатор конечной категории</param>
        /// <returns></returns>
        Task<Category> MergeOneAsync(string fromId, string toId);

        /// <summary>
        ///     Удаляет категорию
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Если есть дочерние категории или товары</exception>
        Task DeleteOneAsync(string id);
    }
}