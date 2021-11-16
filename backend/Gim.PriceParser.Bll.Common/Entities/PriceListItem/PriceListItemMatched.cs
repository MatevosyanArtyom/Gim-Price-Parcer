using System.Collections.Generic;
using Gim.PriceParser.Bll.Common.Entities.Categories;
using Gim.PriceParser.Bll.Common.Entities.PriceLists;
using Gim.PriceParser.Bll.Common.Entities.Products;
using Gim.PriceParser.Bll.Common.Entities.SupplierProducts;

namespace Gim.PriceParser.Bll.Common.Entities.PriceListItem
{
    /// <summary>
    ///     Модель строки прайс-листа, дополненная данными из БД
    /// </summary>
    public class PriceListItemMatched: PriceListItemBase
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
        ///     Прайс-лист
        /// </summary>
        public PriceList PriceList { get; set; }

        /// <summary>
        ///     Идентификатор прайс-листа
        /// </summary>
        public string PriceListId { get; set; }

        /// <summary>
        ///     Сопоставленная номенклатура
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        ///     Идентификатор номенклатуры
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        ///     Похожие товары, найденные подсистемой поиска (Elastic)
        /// </summary>
        public List<ProductSynonym> ProductSynonyms { get; set; } = new List<ProductSynonym>();

        /// <summary>
        ///     Статус поля наименования
        /// </summary>
        public PriceListItemStatus NameStatus { get; set; }

        /// <summary>
        ///     Действие, которое будет применено при загрузке строки в БД
        /// </summary>
        public PriceListItemAction NameAction { get; set; }

        /// <summary>
        ///     Сопоставленная категория 1
        /// </summary>
        public Category Category1 { get; set; }

        /// <summary>
        ///     Идентификатор категории 1
        /// </summary>
        public string Category1Id { get; set; }

        /// <summary>
        ///     Статус поля Категория 1
        /// </summary>
        public PriceListItemStatus Category1Status { get; set; }

        /// <summary>
        ///     Действие для поля Категория 1, которое будет выполнено при загрузке строки в БД
        /// </summary>
        public PriceListItemCategoryAction Category1Action { get; set; }

        /// <summary>
        ///     Идентификатор существующей категории, в которую будет добавлен аналог (маппинг, синоним)
        /// </summary>
        public string MapTo1Id { get; set; }

        /// <summary>
        ///     Сопоставленная категория 2
        /// </summary>
        public Category Category2 { get; set; }

        /// <summary>
        ///     Идентификатор категории 2
        /// </summary>
        public string Category2Id { get; set; }

        /// <summary>
        ///     Статус поля Категория 2
        /// </summary>
        public PriceListItemStatus Category2Status { get; set; }

        /// <summary>
        ///     Действие для поля Категория 2, которое будет выполнено при загрузке строки в БД
        /// </summary>
        public PriceListItemCategoryAction Category2Action { get; set; }

        /// <summary>
        ///     Идентификатор существующей категории, в которую будет добавлен аналог (маппинг, синоним)
        /// </summary>
        public string MapTo2Id { get; set; }

        /// <summary>
        ///     Сопоставленная категория 3
        /// </summary>
        public Category Category3 { get; set; }

        /// <summary>
        ///     Идентификатор категории 3
        /// </summary>
        public string Category3Id { get; set; }

        /// <summary>
        ///     Статус поля Категория 3
        /// </summary>
        public PriceListItemStatus Category3Status { get; set; }

        /// <summary>
        ///     Действие для поля Категория 3, которое будет выполнено при загрузке строки в БД
        /// </summary>
        public PriceListItemCategoryAction Category3Action { get; set; }

        /// <summary>
        ///     Идентификатор существующей категории, в которую будет добавлен аналог (маппинг, синоним)
        /// </summary>
        public string MapTo3Id { get; set; }

        /// <summary>
        ///     Сопоставленная категория 4
        /// </summary>
        public Category Category4 { get; set; }

        /// <summary>
        ///     Идентификатор категории 4
        /// </summary>
        public string Category4Id { get; set; }

        /// <summary>
        ///     Статус поля Категория 4
        /// </summary>
        public PriceListItemStatus Category4Status { get; set; }

        /// <summary>
        ///     Действие для поля Категория 4, которое будет выполнено при загрузке строки в БД
        /// </summary>
        public PriceListItemCategoryAction Category4Action { get; set; }

        /// <summary>
        ///     Идентификатор существующей категории, в которую будет добавлен аналог (маппинг, синоним)
        /// </summary>
        public string MapTo4Id { get; set; }

        /// <summary>
        ///     Сопоставленная категория 5
        /// </summary>
        public Category Category5 { get; set; }

        /// <summary>
        ///     Идентификатор категории 5
        /// </summary>
        public string Category5Id { get; set; }

        /// <summary>
        ///     Статус поля Категория 5
        /// </summary>
        public PriceListItemStatus Category5Status { get; set; }

        /// <summary>
        ///     Действие для поля Категория 5, которое будет выполнено при загрузке строки в БД
        /// </summary>
        public PriceListItemCategoryAction Category5Action { get; set; }

        /// <summary>
        ///     Идентификатор существующей категории, в которую будет добавлен аналог (маппинг, синоним)
        /// </summary>
        public string MapTo5Id { get; set; }

        /// <summary>
        ///     Сопоставленная номенклатура поставщика
        /// </summary>
        public SupplierProduct SupplierProduct { get; set; }

        /// <summary>
        ///     Идентификатор номенклатуры поставщика
        /// </summary>
        public string SupplierProductId { get; set; }

        /// <summary>
        ///     Сопоставленные характеристики и их значения
        /// </summary>
        public List<PriceListItemPropertyMatched> Properties { get; set; } = new List<PriceListItemPropertyMatched>();

        /// <summary>
        ///     Список изображений
        /// </summary>
        public List<PriceListItemImage> Images { get; set; } = new List<PriceListItemImage>();

        /// <summary>
        ///     Статус строки прайс-листа
        /// </summary>
        public PriceListItemStatus Status { get; set; }

        public HashSet<PriceListItemError> Errors { get; set; } = new HashSet<PriceListItemError>();

        /// <summary>
        ///     Пропустить строку при загрузке в БД
        /// </summary>
        public bool Skip { get; set; }

        /// <summary>
        ///     Идентификатор конечной категории
        /// </summary>
        public string CategoryId =>
            string.IsNullOrWhiteSpace(Category5Id)
                ? string.IsNullOrWhiteSpace(Category4Id)
                    ? string.IsNullOrWhiteSpace(Category3Id)
                        ? string.IsNullOrWhiteSpace(Category2Id)
                            ? Category1Id
                            : Category2Id
                        : Category3Id
                    : Category4Id
                : Category5Id;
    }
}
