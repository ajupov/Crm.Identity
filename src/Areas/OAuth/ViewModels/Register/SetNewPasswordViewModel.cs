using System;

namespace Crm.Identity.Areas.OAuth.ViewModels.Register
{
    public class ResetPasswordConfirmationViewModel
    {
        public ResetPasswordConfirmationViewModel(Guid tokenId, string code, bool isPasswordsNotEqual)
        {
            TokenId = tokenId;
            Code = code;
            IsPasswordsNotEqual = isPasswordsNotEqual;
        }

        public Guid TokenId { get; }

        public string Code { get; }

        public bool IsPasswordsNotEqual { get; }
    }
}