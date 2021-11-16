using System.Threading.Tasks;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    /// <summary>
    ///     Интерфейс для работы с архивируемыми сущностями
    /// </summary>
    public interface IArchivableDao
    {
        /// <summary>
        ///     Архивирует один элемент коллекции
        /// </summary>
        /// <param name="id">Идентификатор элемента</param>
        /// <returns></returns>
        Task ToArchiveOneAsync(string id);

        /// <summary>
        ///     Возвращает один элемент коллекции в действующие
        /// </summary>
        /// <param name="id">Идентификатор элемента</param>
        /// <returns></returns>
        Task FromArchiveOneAsync(string id);
    }
}