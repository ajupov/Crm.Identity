using System.ComponentModel.DataAnnotations;
using Crm.Identity.OAuth.Attributes.Validation;

namespace Crm.Identity.OAuth.Models.ChangePassword
{
    public class PostChangePasswordRequest
    {
        [CountryValidation]
        public string Country { get; set; }

        [Required]
        [StringLength(256)]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        [Required]
        [StringLength(256)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(256)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(256)]
        [DataType(DataType.Password)]
        public string NewPasswordConfirmation { get; set; }
    }
}
