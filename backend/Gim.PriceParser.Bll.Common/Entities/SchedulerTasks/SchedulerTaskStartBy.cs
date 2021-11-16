namespace Gim.PriceParser.Bll.Common.Entities.SchedulerTasks
{
    /// <summary>
    ///     Тип инициации
    /// </summary>
    public enum SchedulerTaskStartBy
    {
        Unknown = 0,

        /// <summary>
        ///     Сообщение электронной почты
        /// </summary>
        Email = 1,

        /// <summary>
        ///     По расписанию
        /// </summary>
        Schedule = 2
    }
}