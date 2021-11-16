﻿using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Account
{
    public class LoginModel
    {
        [Required]
        [RegularExpression(@"(?i)^[\w!#$%&'*+/=?`{|}~^-]+(?:\.[\w!#$%&'*+/=?`{|}~^-]+)*@(?:[A-Z0-9-]+\.)+[A-Z]{2,6}$",
            ErrorMessage = "Invalid e-mail format")]
        public string Email { get; set; }

        [Required]
        [StringLength(24, MinimumLength = 8)]
        [RegularExpression(@"^.*(?=.*\d)(?=.*[a-zA-Z]).*$", ErrorMessage =
            "Password must contain at least one number and alphabetical character")]
        public string Password { get; set; }
    }
}