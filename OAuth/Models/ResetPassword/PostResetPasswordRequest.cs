using System.ComponentModel.DataAnnotations;

namespace Ajupov.Identity.OAuth.Models.ResetPassword
{
    public class PostResetPasswordRequest
    {
        [Required]
        [StringLength(256)]
        [DataType(DataType.Text)]
        public string Login { get; set; }
    }
}