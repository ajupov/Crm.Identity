using System;

namespace Crm.Identity.OAuth.ViewModels.VerifyPhone
{
    public class VerifyPhoneViewModel
    {
        public VerifyPhoneViewModel(Guid tokenId, string callbackUri, string code, bool isInvalidCode)
        {
            TokenId = tokenId;
            CallbackUri = callbackUri;
            Code = code;
            IsInvalidCode = isInvalidCode;
        }

        public Guid TokenId { get; }

        public string CallbackUri { get; }

        public bool IsInvalidCode { get; }

        public string Code { get; }
    }
}
