using System;

namespace Ajupov.Identity.OAuth.ViewModels.Register
{
    public class ResetPasswordConfirmationViewModel
    {
        public ResetPasswordConfirmationViewModel(Guid tokenId, string code)
        {
            TokenId = tokenId;
            Code = code;
        }

        public Guid TokenId { get; }

        public string Code { get; }
    }
}