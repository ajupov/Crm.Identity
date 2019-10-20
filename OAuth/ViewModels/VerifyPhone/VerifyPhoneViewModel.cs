using System;

namespace Ajupov.Identity.OAuth.ViewModels.VerifyPhone
{
    public class VerifyPhoneViewModel
    {
        public VerifyPhoneViewModel(Guid tokenId, string callbackUri, bool isInvalidCode)
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