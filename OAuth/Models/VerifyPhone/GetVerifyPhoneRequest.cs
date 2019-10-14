using System;
using System.ComponentModel.DataAnnotations;

namespace Ajupov.Identity.OAuth.Models.VerifyPhone
{
    public class GetVerifyPhoneRequest
    {
        public GetVerifyPhoneRequest(Guid tokenId, string callbackUri, bool isInvalidCode = false)
        {
            TokenId = tokenId;
            CallbackUri = callbackUri;
            IsInvalidCode = isInvalidCode;
        }

        [Required]
        public Guid TokenId { get; }

        public string CallbackUri { get; }
        
        public bool IsInvalidCode { get; }
    }
}