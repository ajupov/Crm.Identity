namespace Ajupov.Identity.OAuth.Models.ResetPassword
{
    public class PostResetPasswordResponse
    {
        public PostResetPasswordResponse(bool isInvalidLogin)
        {
            IsInvalidLogin = isInvalidLogin;
        }

        public bool IsInvalidLogin { get; }
    }
}