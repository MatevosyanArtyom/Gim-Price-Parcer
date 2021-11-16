namespace Gim.PriceParser.Bll.Common.Entities
{
    /// <summary>
    ///     Представление файла
    /// </summary>
    public class GimFile
    {
        /// <summary>
        ///     Имя файла, включая расширение
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Размер файла в байтах
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        ///     Строка с содержимым файла в кодировке Base 64
        /// </summary>
        public string Data { get; set; }
    }
}