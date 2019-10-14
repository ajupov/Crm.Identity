using System;
using ServiceStack.DataAnnotations;

namespace Ajupov.Identity.OAuth.Models.VerifyPhone
{
    public class PostVerifyPhoneRequest
    {
        [Required]
        public Guid TokenId { get; }
        
        [Required]
        public string Code { get; }

        public string CallbackUri { get; }
    }
}