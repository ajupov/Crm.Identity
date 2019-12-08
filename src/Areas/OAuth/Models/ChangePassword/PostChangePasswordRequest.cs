using System.ComponentModel.DataAnnotations;
using Crm.Identity.Areas.OAuth.Attributes.Validation;

namespace Crm.Identity.Areas.OAuth.Models.ChangePassword
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