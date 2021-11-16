using System.ComponentModel.DataAnnotations;

namespace Gim.PriceParser.WebApi.Models.Supplier
{
    public class ContactPersonDto
    {
        [Required]
        public string Name { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Skype { get; set; }
        public bool HasTelegram { get; set; }
        public bool HasViber { get; set; }
        public bool HasWhatsApp { get; set; }
        public string Availability { get; set; }
    }
}