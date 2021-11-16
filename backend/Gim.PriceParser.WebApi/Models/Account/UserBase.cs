using System;
using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities.Users;

namespace Gim.PriceParser.WebApi.Models.Account
{
    public abstract class UserBase
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string RoleId { get; set; }

        public string PhoneNumber { get; set; }
        public bool HasSuppliersAccess { get; set; }
        public bool HasFullAccess { get; set; }
        public bool HasUsersAccess { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsArchived { get; set; }
        public GimUserStatus Status { get; set; }
    }
}