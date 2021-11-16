using System;
using Gim.PriceParser.Bll.Common.Entities.ProcessingRules;
using Gim.PriceParser.Bll.Common.Entities.SchedulerTasks;
using Gim.PriceParser.Bll.Common.Entities.Suppliers;
using Gim.PriceParser.Bll.Common.Entities.Users;

namespace Gim.PriceParser.Bll.Common.Entities.PriceLists
{
    /// <summary>
    ///     Прайс-лист
    /// </summary>
    public class PriceList
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
        ///     Поставщик
        /// </summary>
        public Supplier Supplier { get; set; }

        /// <summary>
        ///     Идентификатор поставщика
        /// </summary>
        public string SupplierId { get; set; }

        /// <summary>
        ///     Задача планировщика
        /// </summary>
        public SchedulerTask SchedulerTask { get; set; }

        /// <summary>
        ///     Идентификатор задачи планировщика
        /// </summary>
        public string SchedulerTaskId { get; set; }

        /// <summary>
        ///     Правила обработки
        /// </summary>
        public ProcessingRule ProcessingRule { get; set; }

        /// <summary>
        ///     Идентификатор правил обработки
        /// </summary>
        public string ProcessingRuleId { get; set; }

        /// <summary>
        ///     Источник правил для обработки прайс-листа
        /// </summary>
        //public RulesSource RulesSource { get; set; }

        /// <summary>
        ///     Код правил (когда RulesSource == Code)
        /// </summary>
        //public string Code { get; set; }

        /// <summary>
        ///     Файл правил (когда RulesSource == File)
        /// </summary>
        //public GimFile CodeFile { get; set; }

        /// <summary>
        ///     Файл прайс-листа
        /// </summary>
        public GimFile PriceListFile { get; set; }

        /// <summary>
        ///     Есть необработанные ошибки артикула
        /// </summary>
        public bool HasUnprocessedCodeErrors { get; set; }

        /// <summary>
        ///     Есть необработанные ошибки наименования
        /// </summary>
        public bool HasUnprocessedNameErrors { get; set; }

        /// <summary>
        ///     Есть необработанные ошибки цены
        /// </summary>
        public bool HasUnprocessedPriceErrors { get; set; }

        /// <summary>
        ///     Есть какие-либо необработанные элементы (артикул, наименование, цена, категория)
        /// </summary>
        public bool HasUnprocessedErrors { get; set; }

        /// <summary>
        ///     Есть ошибки в характеристиках
        /// </summary>
        public bool HasPropertiesErrors { get; set; }

        /// <summary>
        ///     Создать характеристики, если они не были сопоставлены
        /// </summary>
        public bool CreateProperties { get; set; }

        /// <summary>
        ///     Статус обработки
        /// </summary>
        public PriceListStatus Status { get; set; }

        /// <summary>
        ///     Дата создания
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     Пользователь, добавивший прайс-лист
        /// </summary>
        public GimUser Author { get; set; }

        /// <summary>
        ///     Идентификатор пользователя, добавившего прайс-лист
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        ///     Дата установки статуса
        /// </summary>
        public DateTime StatusDate { get; set; }

        /// <summary>
        ///     Дата обработки
        /// </summary>
        public DateTime? ParsedDate { get; set; }
    }
}