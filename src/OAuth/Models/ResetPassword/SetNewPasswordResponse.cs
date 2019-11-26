namespace Crm.Identity.OAuth.Models.ResetPassword
{
    public class SetNewPasswordResponse
    {
        public SetNewPasswordResponse(bool isInvalidToken)
        {
            IsInvalidToken = isInvalidToken;
        }

        public bool IsInvalidToken { get; }
    }
}