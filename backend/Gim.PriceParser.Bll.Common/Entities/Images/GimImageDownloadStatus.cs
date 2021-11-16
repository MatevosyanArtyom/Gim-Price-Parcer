namespace Gim.PriceParser.Bll.Common.Entities.Images
{
    /// <summary>
    ///     Статус загрузки изображения
    /// </summary>
    public enum GimImageDownloadStatus
    {
        Unknown = 0,

        /// <summary>
        ///     Еще не загружалось
        /// </summary>
        NotDownloaded = 1,

        /// <summary>
        ///     Загружается
        /// </summary>
        Downloading = 2,

        /// <summary>
        ///     Загрузка успешна
        /// </summary>
        DownloadSuccess = 3,

        /// <summary>
        ///     Загрузка неуспешна
        /// </summary>
        DownloadFail = 4
    }
}