using System.ComponentModel.DataAnnotations;

namespace Ajupov.Identity.OAuth.Models.ResetPassword
{
    public class GetResetPasswordRequest
    {
        [Required]
        [StringLength(256)]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        public bool IsInvalidLogin { get; set; }
    }
}