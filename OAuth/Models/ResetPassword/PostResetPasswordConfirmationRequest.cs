using System;
using System.ComponentModel.DataAnnotations;

namespace Ajupov.Identity.OAuth.Models.ResetPassword
{
    public class PostResetPasswordConfirmationRequest
    {
        [Required]
        public Guid TokenId { get; set; }

        [Required]
        public string Code { get; set; }

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