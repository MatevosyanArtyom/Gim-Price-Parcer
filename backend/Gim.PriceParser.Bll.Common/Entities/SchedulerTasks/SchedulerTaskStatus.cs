namespace Gim.PriceParser.Bll.Common.Entities.SchedulerTasks
{
    /// <summary>
    ///     Статус задачи планировщика
    /// </summary>
    public enum SchedulerTaskStatus
    {
        Unknown = 0,

        /// <summary>
        ///     Активная
        /// </summary>
        Active = 1,

        /// <summary>
        ///     Неактивная
        /// </summary>
        Inactive = 2,

        /// <summary>
        ///     Новая
        /// </summary>
        New = 3
    }
}