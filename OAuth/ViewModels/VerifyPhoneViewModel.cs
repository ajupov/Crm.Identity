using System;

namespace Ajupov.Identity.OAuth.ViewModels
{
    public class VerifyPhoneViewModel
    {
        public VerifyPhoneViewModel(Guid tokenId, string callbackUri, bool isInvalidCode = false)
        {
            TokenId = tokenId;
            CallbackUri = callbackUri;
            IsInvalidCode = isInvalidCode;
        }

        public Guid TokenId { get; }

        public string CallbackUri { get; }

        public bool IsInvalidCode { get; }
    }
}