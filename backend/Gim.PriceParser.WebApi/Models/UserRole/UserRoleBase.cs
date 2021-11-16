using System;
using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.UserRole
{
    public class UserRoleBase
    {
        [Required]
        public string Name { get; set; }

        public bool IsMainAdmin { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsArchived { get; set; }

        [Required]
        public AccessRightsDto AccessRights { get; set; }
    }
}