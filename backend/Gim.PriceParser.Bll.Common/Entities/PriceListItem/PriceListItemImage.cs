using Gim.PriceParser.Bll.Common.Entities.Images;

namespace Gim.PriceParser.Bll.Common.Entities.PriceListItem
{
    /// <summary>
    ///     Сопоставленный элемент изображения товара
    /// </summary>
    public class PriceListItemImage
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Числовой идентификатор
        /// </summary>
        public long SeqId { get; set; }

        /// <summary>
        ///     Идентификатор строки прайс-листа
        /// </summary>
        public string PriceListItemId { get; set; }

        /// <summary>
        ///     Сопоставленное изображение номенклатуры
        /// </summary>
        public GimImage Image { get; set; }

        /// <summary>
        ///     Идентификатор изображения
        /// </summary>
        public string ImageId { get; set; }

        /// <summary>
        ///     Ссылка на изображение
        /// </summary>
        public string Url { get; set; }
    }
}