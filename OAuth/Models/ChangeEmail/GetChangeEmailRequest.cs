using System.ComponentModel.DataAnnotations;

namespace Ajupov.Identity.OAuth.Models.ChangeEmail
{
    public class GetChangeEmailRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(256)]
        [EmailAddress]
        public string OldEmail { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(256)]
        [EmailAddress]
        public string NewEmail { get; set; }

        public bool IsInvalidCredentials { get; set; }
    }
}