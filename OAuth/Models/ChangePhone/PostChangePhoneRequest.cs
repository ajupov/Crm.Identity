using System.ComponentModel.DataAnnotations;
using Crm.Identity.OAuth.Attributes.Validation;

namespace Crm.Identity.OAuth.Models.ChangePhone
{
    public class PostChangePhoneRequest
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        [Phone]
        public string OldPhone { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        [Phone]
        public string NewPhone { get; set; }

        [Required]
        [StringLength(256)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [CountryValidation]
        public string Country { get; set; }
    }
}