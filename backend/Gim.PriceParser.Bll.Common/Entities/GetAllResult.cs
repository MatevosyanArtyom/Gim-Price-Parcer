using System.Collections.Generic;

namespace Gim.PriceParser.Bll.Common.Entities
{
    /// <summary>
    ///     Результат постраничной выборки элементов
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GetAllResult<T>
    {
        /// <summary>
        ///     Общее количество элементов
        /// </summary>
        public long Count { get; set; }

        /// <summary>
        ///     Элементы текущей страницы
        /// </summary>
        public List<T> Entities { get; set; } = new List<T>();
    }
}