using System;
using System.Collections.Generic;

namespace Gim.PriceParser.Bll.Common.Entities.Categories
{
    /// <summary>
    ///     Категория товаров
    /// </summary>
    public class Category
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Родительская категория
        /// </summary>
        public Category Parent { get; set; }

        /// <summary>
        ///     Идентификатор родительской категории
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        ///     Полный путь к категории.
        ///     Состоит из списка Id, начиная с корня, разделенных символом '/'
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     Массив родительских категорий (путь)
        /// </summary>
        public List<string> Ancestors { get; set; } = new List<string>();

        /// <summary>
        ///     Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Список маппингов (варианты написания наименования)
        /// </summary>
        public IEnumerable<CategoryMappingItem> Mappings { get; set; } = new List<CategoryMappingItem>();

        /// <summary>
        ///     Позиция. Используется для сортировки категорий
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        ///     Имеет ли категория дочерние элементы
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        ///     Количество товаров
        /// </summary>
        public long ProductsCount { get; set; }

        /// <summary>
        ///     Дата и время изменения
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        ///     Статус
        /// </summary>
        public EntityStatus Status { get; set; }

        /// <summary>
        ///     Версия
        /// </summary>
        public string Version { get; set; }
    }
}