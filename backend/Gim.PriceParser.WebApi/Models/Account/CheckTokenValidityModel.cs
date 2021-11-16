using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities.Users;

namespace Gim.PriceParser.WebApi.Models.Account
{
    /// <summary>
    ///     Модель для проверки токена перед сменой пароля
    /// </summary>
    public class CheckTokenValidityModel
    {
        /// <summary>
        ///     Валидируемый токен
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        ///     Признак верного токена
        /// </summary>
        public bool IsTokenValid { get; set; }

        /// <summary>
        ///     Эл. почта пользователя, которому меняется пароль
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Статус пользователя
        /// </summary>
        public GimUserStatus Status { get; set; }
    }
}