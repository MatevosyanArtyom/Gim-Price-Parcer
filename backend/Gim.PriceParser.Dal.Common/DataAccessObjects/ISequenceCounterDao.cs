using System.Threading.Tasks;

namespace Gim.PriceParser.Dal.Common.DataAccessObjects
{
    /// <summary>
    ///     Интерфейс для работы с последовательностями
    /// </summary>
    public interface ISequenceCounterDao
    {
        /// <summary>
        ///     Получает значение счетчика
        /// </summary>
        /// <param name="name">Имя коллекции</param>
        /// <returns>Значение счетчика</returns>
        Task<long> GetCounterAsync(string name);

        /// <summary>
        ///     Устанавливает значение счетчика
        /// </summary>
        /// <param name="name">Имя коллекции</param>
        /// <param name="counter">Значение счетчика</param>
        /// <returns></returns>
        Task SetCounterAsync(string name, long counter);
    }
}