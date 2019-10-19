using System.ComponentModel.DataAnnotations;

namespace Ajupov.Identity.OAuth.Models.ChangePassword
{
    public class GetChangePasswordRequest
    {
        [Required]
        [StringLength(256)]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        public bool IsInvalidCredentials { get; set; }
    }
}