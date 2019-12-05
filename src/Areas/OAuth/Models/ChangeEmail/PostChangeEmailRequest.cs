using System.ComponentModel.DataAnnotations;

namespace Crm.Identity.Areas.OAuth.Models.ChangeEmail
{
    public class PostChangeEmailRequest
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

        [Required]
        [StringLength(256)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}