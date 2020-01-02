using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Identity.OAuth.Models.VerifyEmail
{
    public class VerifyEmailRequest
    {
        [Required]
        public Guid TokenId { get; set; }

        [Required]
        public string Code { get; set; }
    }
}