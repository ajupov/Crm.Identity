using System;
using System.ComponentModel.DataAnnotations;

namespace Ajupov.Identity.OAuth.Models.ResetPassword
{
    public class ResetPasswordConfirmationRequest
    {
        [Required]
        public Guid TokenId { get; set; }

        [Required]
        public string Code { get; set; }
    }
}