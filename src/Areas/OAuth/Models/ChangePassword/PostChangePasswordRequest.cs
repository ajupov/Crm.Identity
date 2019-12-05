using System.ComponentModel.DataAnnotations;

namespace Crm.Identity.Areas.OAuth.Models.ChangePassword
{
    public class PostChangePasswordRequest
    {
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