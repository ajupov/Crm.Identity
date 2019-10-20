namespace Ajupov.Identity.src.OAuth.Models.ChangePassword
{
    public class PostChangePasswordResponse
    {
        public PostChangePasswordResponse(bool isInvalidCredentials)
        {
            IsInvalidCredentials = isInvalidCredentials;
        }

        public bool IsInvalidCredentials { get; }
    }
}