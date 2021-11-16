namespace Gim.PriceParser.Bll.Common.Entities.Images
{
    /// <summary>
    ///     Представление изображения
    /// </summary>
    public class GimImage
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Идентификатор товара
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        ///     Признак главного изображения
        /// </summary>
        public bool IsMain { get; set; }

        /// <summary>
        ///     Признак публикации
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        ///     Имя файла, включая расширение
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Адрес файла (если получен по сети)
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     Размер файла в байтах
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        ///     Данные файла
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        ///     Статус загрузки
        /// </summary>
        public GimImageDownloadStatus Status { get; set; }
    }
}