using System;
using ServiceStack.DataAnnotations;

namespace Ajupov.Identity.OAuth.Models.VerifyPhone
{
    public class PostVerifyPhoneRequest
    {
        [Required]
        public Guid TokenId { get; set; }

        [Required]
        public string Code { get; set; }

        public string CallbackUri { get; set; }
    }
}