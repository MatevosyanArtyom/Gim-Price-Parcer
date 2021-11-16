using System;
using Gim.PriceParser.Bll.Common.Entities.Suppliers;

namespace Gim.PriceParser.Bll.Common.Entities.SchedulerTasks
{
    /// <summary>
    ///     Задача планировщика на загрузку прайса
    /// </summary>
    public class SchedulerTask
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Поставщик
        /// </summary>
        public Supplier Supplier { get; set; }

        /// <summary>
        ///     Идентификатор поставщика
        /// </summary>
        public string SupplierId { get; set; }

        /// <summary>
        ///     Способ интеграции
        /// </summary>
        public IntegrationMethod IntegrationMethod { get; set; }

        /// <summary>
        ///     Требуется запрос
        /// </summary>
        public bool RequestRequired { get; set; }

        /// <summary>
        ///     Способ инициации
        /// </summary>
        public SchedulerTaskStartBy StartBy { get; set; }

        /// <summary>
        ///     Адреса отправители сообщений, при получении писем от которых должна инициироваться задача
        ///     Адреса разделены символом ;
        /// </summary>
        public string Emails { get; set; }

        /// <summary>
        ///     Расписание в формате cron
        /// </summary>
        public string Schedule { get; set; }

        /// <summary>
        ///     Правила обработки данных
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        ///     Статус
        /// </summary>
        public SchedulerTaskStatus Status { get; set; }

        /// <summary>
        ///     Версия
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        ///     Дата и время изменения
        /// </summary>
        public DateTime Modified { get; set; }
    }
}