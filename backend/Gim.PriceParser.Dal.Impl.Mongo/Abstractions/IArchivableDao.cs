using System.Threading.Tasks;
using Gim.PriceParser.Dal.Impl.Mongo.DataObjects.Abstractions;
using MongoDB.Driver;

namespace Gim.PriceParser.Dal.Impl.Mongo.Abstractions
{
    /// <summary>
    ///     Интерфейс для работы с архивируемыми сущностями
    /// </summary>
    internal interface IArchivableDao<TDo> where TDo : IEntityWithIdDo, IEntityArchivableDo
    {
        IMongoCollection<TDo> Col { get; set; }

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