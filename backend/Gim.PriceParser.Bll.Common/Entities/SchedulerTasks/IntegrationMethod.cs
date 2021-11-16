namespace Gim.PriceParser.Bll.Common.Entities.SchedulerTasks
{
    /// <summary>
    ///     Способ интеграции
    /// </summary>
    public enum IntegrationMethod
    {
        Unknown = 0,

        /// <summary>
        ///     API
        /// </summary>
        Api = 1,

        /// <summary>
        ///     Электронная почта
        /// </summary>
        Email = 2,

        /// <summary>
        ///     Файл
        /// </summary>
        File = 3
    }
}