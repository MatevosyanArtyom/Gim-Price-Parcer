using System.Collections.Generic;
using System.Threading.Tasks;
using Gim.PriceParser.Bll.Common.Entities;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    /// <summary>
    ///     Базовый интерфейс работы с коллекцией
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDaoBase<T>
    {
        /// <summary>
        ///     Получает элементы постранично
        /// </summary>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <returns></returns>
        Task<GetAllResult<T>> GetManyAsync(int page = 0, int pageSize = 0);

        Task<List<string>> GetIds();

        /// <summary>
        ///     Возвращает новый сгенерированный ObjectId
        /// </summary>
        /// <returns></returns>
        string GenerateNewObjectId();

        /// <summary>
        ///     Получает один элемент из коллекции
        /// </summary>
        /// <param name="id">Идентификатор элемента</param>
        /// <returns></returns>
        Task<T> GetOneAsync(string id);

        /// <summary>
        ///     Добавляет один элемент в коллекцию
        /// </summary>
        /// <param name="entity">Элемент для добавления</param>
        /// <returns></returns>
        Task<T> AddOneAsync(T entity);

        /// <summary>
        ///     Добавляет список элементов в коллекцию
        /// </summary>
        /// <param name="entities">Список элементов для добавления</param>
        /// <returns></returns>
        Task<List<T>> AddManyAsync(List<T> entities);

        /// <summary>
        ///     Обновляет один элемент в коллекции
        /// </summary>
        /// <param name="entity">Элемент для обновления</param>
        /// <returns></returns>
        Task<T> UpdateOneAsync(T entity);

        /// <summary>
        ///     Удаляет один элемент из коллекции
        /// </summary>
        /// <param name="id">Идентификатор элемента</param>
        /// <returns></returns>
        Task DeleteOneAsync(string id);
    }
}