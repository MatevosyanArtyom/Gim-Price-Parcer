using System.ComponentModel.DataAnnotations;
using Gim.PriceParser.Bll.Common.Entities.UserRoles;

namespace Gim.PriceParser.WebApi.Models.UserRole
{
    public class AccessRightsDto
    {
        [Required]
        public AccessRightMode Suppliers { get; set; } = new AccessRightMode();
        

        [Required]
        public AccessRightMode PriceListAdd { get; set; } = new AccessRightMode();

        [Required]
        public AccessRightMode PriceLists { get; set; } = new AccessRightMode();

        [Required]
        public AccessRightMode CommitedPriceLists { get; set; } = new AccessRightMode();


        [Required]
        public AccessRightMode Products { get; set; } = new AccessRightMode();


        [Required]
        public AccessRightMode UserRoles { get; set; } = new AccessRightMode();

        [Required]
        public AccessRightMode Users { get; set; } = new AccessRightMode();

        [Required]
        public AccessRightMode Categories { get; set; } = new AccessRightMode();

        [Required]
        public AccessRightMode Properties { get; set; } = new AccessRightMode();

        [Required]
        public AccessRightMode ProcessingRules { get; set; } = new AccessRightMode();
    }
}